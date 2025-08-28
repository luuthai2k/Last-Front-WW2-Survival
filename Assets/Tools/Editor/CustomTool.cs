﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CustomTool : EditorWindow
{
    [SerializeField]
    string lastScene = "";

    [SerializeField]
    int targetScene = 0;

    [SerializeField]
    string waitScene = null;

    //	[SerializeField] bool hasPlayed = false;
    [MenuItem("Edit/Play From Scene %`")]
    public static void Run()
    {
        EditorWindow.GetWindow<CustomTool>();
    }

    static string[] sceneNames;

    static EditorBuildSettingsScene[] scenes;

    void OnEnable()
    {
        scenes = EditorBuildSettings.scenes;
        sceneNames =
            scenes
                .Select(x =>
                    AsSpacedCamelCase(Path.GetFileNameWithoutExtension(x.path)))
                .ToArray();
        //  sceneNames = { 'Splash','Demo2' };
    }

    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            if (null == waitScene && !string.IsNullOrEmpty(lastScene))
            {
                EditorSceneManager.OpenScene (lastScene);
                lastScene = null;
            }
        }
    }

    void OnGUI()
    {
        if (EditorApplication.isPlaying)
        {
            if (EditorApplication.currentScene == waitScene)
            {
                waitScene = null;
            }
            return;
        }

        if (EditorApplication.currentScene == waitScene)
        {
            EditorApplication.isPlaying = true;
        }
        if (null == sceneNames) return;
        targetScene = EditorGUILayout.Popup(targetScene, sceneNames);
        if (GUILayout.Button("Play"))
        {
            lastScene = EditorApplication.currentScene;
            waitScene = scenes[targetScene].path;
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene (waitScene);
        }
    }

    public string AsSpacedCamelCase(string text)
    {
        System.Text.StringBuilder sb =
            new System.Text.StringBuilder(text.Length * 2);
        sb.Append(char.ToUpper(text[0]));
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ') sb.Append(' ');
            sb.Append(text[i]);
        }
        return sb.ToString();
    }

    [MenuItem("Tools/Export Sprite From multi Sprite")]
    static void ExportSpriteFromMultiSprite()
    {
        Dictionary<string, Sprite> dicSprites =
            new Dictionary<string, Sprite>();
        var sprites =
            AssetDatabase
                .LoadAllAssetRepresentationsAtPath("Assets/Images/SpriteAtlasTexture-bricks (Group 0)-1024x2048-fmt4.png");

        Debug.Log(sprites.Length);

        // Sprite[] sprites = new Sprite[obj.Length];
        string directoryPath =
            Application.persistentDataPath + "/" + "Images" + "/";
        for (int i = 0; i < sprites.Length; i++)
        {
            if (!(sprites[i] == null))
            {
                if (dicSprites.ContainsKey(sprites[i].name))
                {
                }
                else
                {
                    dicSprites.Add(sprites[i].name, sprites[i] as Sprite);
                    var sprite = sprites[i] as Sprite;
                    var croppedTexture =
                        new Texture2D((int) sprite.rect.width,
                            (int) sprite.rect.height);
                    var pixels =
                        sprite
                            .texture
                            .GetPixels((int) sprite.textureRect.x,
                            (int) sprite.textureRect.y,
                            (int) sprite.textureRect.width,
                            (int) sprite.textureRect.height);
                    croppedTexture.SetPixels (pixels);
                    croppedTexture.Apply();
                    byte[] bytes = croppedTexture.EncodeToPNG();
                    File
                        .WriteAllBytes(directoryPath + sprites[i].name + ".png",
                        bytes);
                }
            }
        }
    }

    [MenuItem("Edit/Cleanup Missing Scripts")]
    static void CleanupMissingScripts()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            var gameObject = Selection.gameObjects[i];

            // We must use the GetComponents array to actually detect missing components
            var components = gameObject.GetComponents<Component>();

            // Create a serialized object so that we can edit the component list
            var serializedObject = new SerializedObject(gameObject);

            // Find the component list property
            var prop = serializedObject.FindProperty("m_Component");

            // Track how many components we've removed
            int r = 0;

            // Iterate over all components
            for (int j = 0; j < components.Length; j++)
            {
                // Check if the ref is null
                if (components[j] == null)
                {
                    // If so, remove from the serialized component array
                    prop.DeleteArrayElementAtIndex(j - r);

                    // Increment removed count
                    r++;
                }
            }

            // Apply our changes to the game object
            serializedObject.ApplyModifiedProperties();
        }
    }

    [MenuItem("Tools/Clear Cache")]
    static void ClearCache()
    {
        Caching.ClearCache();
    }

    [MenuItem("Tools/PlayerPrefs DeleteAll")]
    static void PlayerPrefsDeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Clear Data")]
    static void ClearData()
    {
        string dataPath =
            Path.Combine(Application.persistentDataPath, "data.dat");
        File.Delete (dataPath);
        dataPath = Path.Combine(Application.persistentDataPath, "ads_data.dat");
        File.Delete (dataPath);
         dataPath =
           Path.Combine(Application.persistentDataPath, "dataBackUp.dat");
        File.Delete(dataPath);
    }

    [MenuItem("Tools/Change Name")]
    static void ChangeChildName()
    {
        for (int i = 0; i < Selection.activeTransform.childCount; i++)
        {
            Selection.activeTransform.GetChild(i).name = i / 7 + "_" + i % 7;
        }
    }

    [MenuItem("Tools/Remove Icon Sprtie")]
    static void RemoveIconSprtie()
    {
        Debug.Log("Proccessing...");
        var gos = Selection.gameObjects[0].GetComponentsInChildren<Image>(true);
        for (int i = 0; i < gos.Length; i++)
        {
            if (gos[i].gameObject.name != "Icon") continue;
            gos[i].sprite = null;
        }
        Debug.Log("Done...");
    }
}

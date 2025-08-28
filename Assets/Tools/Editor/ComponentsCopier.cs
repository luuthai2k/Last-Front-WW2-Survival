#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections;

public class ComponentsCopier : EditorWindow
{
    static List<Component> copiedComponents;
    static string[] types = new string[] { "UnityEngine.RectTransform", "UnityEngine.UI.Image" };
    [MenuItem("GameObject/Copy all components %&C")]
    static void Copy()
    {
        copiedComponents = new List<Component>();
        copiedComponents.AddRange( Selection.activeGameObject.GetComponents<Component>());
        for  (int i=0;i< copiedComponents.Count;i++)
        {
            string type = copiedComponents[i].GetType().ToString();
           if (!isTypeCanCop(type))
                copiedComponents.Remove(copiedComponents[i]);
        }
    }
    static bool isTypeCanCop(string t)
    {
        for (int i = 0; i < types.Length; i++)
            if (t == types[i])
                return true;
        return false;
    }


    [MenuItem("GameObject/Paste all components %&P")]
    static void Paste()
    {
        Component[] targetComponents = Selection.activeGameObject.GetComponents<Component>();
        foreach (var target in targetComponents)
        {
            if (!target || copiedComponents == null) continue;
            foreach (var copiedComponent in copiedComponents)
            {
                if (!copiedComponent) continue;
                UnityEditorInternal.ComponentUtility.CopyComponent(copiedComponent);
                if (copiedComponent.GetType() == target.GetType())
                    UnityEditorInternal.ComponentUtility.PasteComponentValues(target);
            }
        }
    }

}
#endif
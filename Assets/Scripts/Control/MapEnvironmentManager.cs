using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnvironmentManager : MonoBehaviour,IMessageHandle
{
  
    //public WeatherType weatherType;
    public Material skyBoxMaterial;
    public Color skyColor=Color.white;
    public Color equatorColor=Color.grey;
    public Color groundColor=Color.black;
    public AudioClip[] audioClips;
    private void Start()
    {
        SetUp();
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnSceneLoaded, this);
    }
    public void Handle(Message message)
    {
        switch (message.type)
        {
            case TeeMessageType.OnSceneLoaded:
                PlayMusic();
                break;
        }
    }
    void SetUp()
    {
        RenderSettings.skybox = skyBoxMaterial;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = skyColor;
        RenderSettings.ambientEquatorColor = equatorColor;
        RenderSettings.ambientGroundColor = groundColor;
        //if (weatherType != WeatherType.None)
        //{
        //    ResourceHelper.Instance.GetWeatherEffect(weatherType);
        //}
    }
    public void PlayMusic()
    {
        if (audioClips.Length == 0) return;
        AudioController.Instance.PlayMusic(audioClips[Random.Range(0, audioClips.Length)]);
    }
    void OnDestroy()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnSceneLoaded, this);
    }
}
public enum WeatherType
{
    None = -1,
    Snow = 0,
    Rain = 1
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    public bool isSfxMute, isMusicMute;
    [SerializeField] private AudioSource sfxSource, musicSource;
    public AudioClip[] sounds;
    public AudioClip[] music;
    private float originVol;
    public Dictionary<string, AudioClip> data = new();
    private void OnEnable()
    {
        originVol = musicSource.volume;
    }
   
    public bool Music
    {
        set
        {
            PlayerPrefs.SetInt("music", value ? 1 : 0);
            isMusicMute = !value;
            if (isMusicMute)
            {
                //DOTween.To(() => musicSource.volume, x => musicSource.volume = x, 0f, 1f).SetEase(Ease.OutQuad).OnComplete(() => musicSource.Stop());
               /* musicSource.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>*/ musicSource.Stop();
                musicSource.volume = 0;
            }
            else
            {
                musicSource.Play();
                musicSource.DOFade(0.5f, 1f).SetEase(Ease.OutQuad)/*.OnComplete(() => musicSource.Stop())*/;
                //DOTween.To(() => musicSource.volume, x => musicSource.volume = x, 1f, 0f).SetEase(Ease.InQuad);


            }
        }
        get { return PlayerPrefs.GetInt("music", 1) == 1; }
    }
    public bool SFX
    {
        set
        {
            PlayerPrefs.SetInt("sfx", value ? 1 : 0);
            isSfxMute = !value;
        }
        get { return PlayerPrefs.GetInt("sfx", 1) == 1; }
    }
    void Start()
    {
        isMusicMute = !Music;
        isSfxMute = !SFX;
        CreateData();
        if (!isMusicMute)
            musicSource.Play();
    }
    void CreateData()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
           
            if (!data.ContainsKey(sounds[i].name))
            {
                data.Add(sounds[i].name, sounds[i]);
            }
            else
            {
                Debug.LogWarning("Key "+ sounds[i].name+" đã tồn tại!");
            }

        }
        for (int i = 0; i < music.Length; i++)
        {

            if (!data.ContainsKey(music[i].name))
            {
                data.Add(music[i].name, music[i]);
            }
            else
            {
                Debug.LogWarning("Key " + music[i].name + " đã tồn tại!");
            }

        }
    }
    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        if (!isSfxMute)
        {
            sfxSource.volume = volume;
            sfxSource.PlayOneShot(clip);
        }
    }
    public void PlaySfx(string soundName,float volume = 1f)
    {
        if (!isSfxMute)
        {
            sfxSource.volume = volume;
            if (data.TryGetValue(soundName, out var soundClip))
            {
                sfxSource.PlayOneShot(soundClip);
            }
            else
            {
                Debug.LogError(soundName + " does not exist in data.");
            }
           
        }
    }
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != null)
            if (musicSource.clip.name == clip.name)
                return;
        musicSource.volume = 0.5f;
        musicSource.clip = clip;
        if (!isMusicMute)
            musicSource.Play();
    }
    public void PlayMusic(string soundName)
    {
        if (musicSource.clip != null && musicSource.clip.name == soundName)
        {
            musicSource.Play();
            return;
        }

        if (data.TryGetValue(soundName, out var soundClip))
        {
            musicSource.volume = 0.5f;
            musicSource.clip = soundClip;
            if (!isMusicMute)
                musicSource.Play();
        }
        else
        {
            Debug.LogError(soundName + " does not exist in data.");
            return;
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
      
    }
    public AudioClip GetSound(string soundName)
    {
        if (data.TryGetValue(soundName, out var soundClip))
        {
            return soundClip;
        }
        return null;
    }
}

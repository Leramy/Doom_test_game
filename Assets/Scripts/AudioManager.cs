using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource music1Source;
    [SerializeField] private AudioSource music2Source;

    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;

    public float crossFadeRate = 1.5f;
    private bool _crossFading;

    [SerializeField] private string introBGMusic;
    [SerializeField] private string levelBGMusic;
    public ManagerStatus status { get; private set; }

    private NetworkService _service;
    private float _musicVolume;

    public float musicVolume
    {
        get { return _musicVolume; }
        set
        {
            _musicVolume = value;
            if(music1Source != null && !_crossFading)
            {
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume;
            }
        }
    }

    public float SoundVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }

    public bool SoundMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    public bool musicMute
    {
        get 
        {
            if (music1Source != null)
            {
                return music1Source.mute;
            }
            return false;
        }
        set
        {
            if (music1Source != null)
            {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }
    }
    public void Startup(NetworkService service)
    {
        Debug.Log("Audio manager starting...");

        _service = service;

        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerPause = true;
        music1Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerVolume = true;

        SoundVolume = 1f;
        musicVolume = 1f;

        status = ManagerStatus.Started;
        _activeMusic = music1Source;
        _inactiveMusic = music2Source;
    }

   public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void PlayIntroMusic()
    {
        PlayMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
    }

    public void PlayLevelMusic()
    {
        PlayMusic(Resources.Load("Music/" + levelBGMusic) as AudioClip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if(_crossFading) { return; }
        StartCoroutine(CrossFadeMusic(clip));
    }

    public IEnumerator CrossFadeMusic (AudioClip clip)
    {
        _crossFading = true;

        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();

        float scaledRate = crossFadeRate * _musicVolume;

        while(_activeMusic.volume>0)
        {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;

            yield return null;
        }

        AudioSource tmp = _activeMusic;
        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;

        _inactiveMusic = tmp;
        _inactiveMusic.Stop();

        _crossFading = false;
    }
    public void StopMusic()
    {
        _activeMusic.Stop();
        _inactiveMusic.Stop();
    }
}

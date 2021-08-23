using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    [SerializeField] private AudioClip sound;
    void Start()
    {
        speedSlider.value = PlayerPrefs.GetFloat("speed", 1);
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false); 
    }

    public void OnSubmitName (string name)
    {
        Debug.Log(name);
    }

    public void OnSpeedValue (float speed)
    {
        PlayerPrefs.SetFloat("speed", speed);
        Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
    }

    public void OnToggle()
    {
        Managers.Audio.SoundMute = !Managers.Audio.SoundMute;
        Managers.Audio.PlaySound(sound);
    }

    public void OnSoundValue(float value)
    {
        Managers.Audio.SoundVolume = value;
        
    }

    public void OnToggleMusic()
    {
        Managers.Audio.musicMute = !Managers.Audio.musicMute;
        Managers.Audio.PlaySound(sound);
    }

    public void OnMusicValue(float value)
    {
        Managers.Audio.musicVolume = value;

    }

    public void OnPlayMusic(int selector)
    {
        Managers.Audio.PlaySound(sound);

        switch (selector)
        {
            case 1:
                Managers.Audio.PlayLevelMusic();
                break;
            case 2:
                Managers.Audio.PlayIntroMusic();
                break;
            case 3:
                Managers.Audio.StopMusic();
                break;
        }

    }
}

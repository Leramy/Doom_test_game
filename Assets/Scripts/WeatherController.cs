using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField] private Material sky;
    [SerializeField] private Light sun;

    private float _fullIntensity;
    private float _cloudValue = 0f;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.WEATHER_UPDATED,OnWeatherUpdated); 
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.WEATHER_UPDATED,OnWeatherUpdated);
    }
    void Start()
    {
        _fullIntensity = sun.intensity;
    }

    private void OnWeatherUpdated()
    {
        SetOvercast(Managers.Weather.cloudValue);

    }

    private void SetOvercast (float value)
    {
        sky.SetFloat("_Blend", value);
        sun.intensity = _fullIntensity - (_fullIntensity * value); 
    }
}

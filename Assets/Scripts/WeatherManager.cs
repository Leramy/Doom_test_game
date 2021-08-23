using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using MiniJSON;
using UnityEngine;

public class WeatherManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public float cloudValue { get; private set; }

    private NetworkService _network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Weather manager staring...");
        _network = service;

        StartCoroutine(_network.GetWeatherJSON(OnJSONDataLoaded));
        status = ManagerStatus.Initializing;
    }

    public void OnXMLDataLoaded(string data)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data);
        XmlNode root = doc.DocumentElement;

        XmlNode node = root.SelectSingleNode("clouds");
        string value = node.Attributes["value"].Value;
        cloudValue = Convert.ToInt32(value) / 100f;
        Debug.Log("Clouds: " + cloudValue);

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        status = ManagerStatus.Started;
    }

    public void OnJSONDataLoaded(string data)
    {
        Dictionary<string, object> dict;

        dict = Json.Deserialize(data) as Dictionary<string, object>;
        Dictionary<string, object> clouds = (Dictionary<string, object>)dict["clouds"];
        cloudValue = (long)clouds["all"] / 100f;
        Debug.Log("Clouds: " + cloudValue);


        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        status = ManagerStatus.Started;
    }



}

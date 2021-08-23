using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    private NetworkService _network;
    private Texture2D _webImage;

    public void Startup(NetworkService service)
    {
        Debug.Log("Image Manager starting...");
        _network = service;
        status = ManagerStatus.Started;
    }

   public void GetWebImage(Action<Texture2D> callback)
    {
        if (_webImage == null)
        {
            StartCoroutine(_network.DownloadImage((Texture2D image) => {
                _webImage = image;
                callback(_webImage);
            }));
        }
        else
            callback(_webImage);
    }
}

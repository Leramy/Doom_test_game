using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLoadingBillboard : MonoBehaviour
{
    public void Operate()
    {
        Managers.Image.GetWebImage(OnWebImage);
    }

    private void OnWebImage(Texture2D image)
    {

        //Color random = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //GetComponent<Renderer>().material.color = random;
        GetComponent<Renderer>().material.mainTexture = image;
    }
}

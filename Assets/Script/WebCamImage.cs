using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamImage : MonoBehaviour
{
    [SerializeField] private RawImage _webCamImg;
    [SerializeField] private int _imgWidth = 640;
    [SerializeField] private int _imgHeight = 480;
    [SerializeField] private int _fps = 30;

    private void Start()
    {
        var webCamTexture = new WebCamTexture(_imgWidth, _imgHeight, _fps);
        _webCamImg.texture = webCamTexture;

        webCamTexture.Play();
    }
}

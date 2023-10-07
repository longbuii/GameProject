using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollingBackGround : MonoBehaviour
{
    [SerializeField]private RawImage rawImage;
    [SerializeField]private float x;

    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(x,0)* Time.deltaTime,rawImage.uvRect.size);

    }


}





   
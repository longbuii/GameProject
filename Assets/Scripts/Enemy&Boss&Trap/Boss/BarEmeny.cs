using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarEmeny : MonoBehaviour
{
    [SerializeField] private Slider slider;
    //[SerializeField] private Transform target;
    //[SerializeField] private Vector3 offset;

    public void UpdateHealthBar(float currntValue, float maxValue)
    {
        slider.value = currntValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = target.position + offset;
    }
}

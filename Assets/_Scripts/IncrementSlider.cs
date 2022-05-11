using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncrementSlider : MonoBehaviour
{
    [SerializeField] private float _increment = 0.05f;

    private Slider _slider;


    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void Increment()
    {
        _slider.value += _increment;
    }

    public void Decrement()
    {
        _slider.value -= _increment;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DoDoDoIt
{
    public class TestSliderValueChange : MonoBehaviour
    {
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            _slider.value += 0.0001f;
        }
    }
}

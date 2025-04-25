using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherWand : MonoBehaviour
{
    //public TemperatureController tempController;
    public WeatherController weatherController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //tempController.AdjustTemperature(+5f);
            weatherController.AdjustTemperature(+5f);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //tempController.AdjustTemperature(-5f);
            weatherController.AdjustTemperature(-5f);
        }
    }
}

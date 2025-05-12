using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class WeatherUIManager : MonoBehaviour
{
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI weatherText;

    private void Start()
    {
        UpdateWeatherUI();
    }

    public void UpdateWeatherUI()
    {
        
        float temp = GameManager.Instance.savedTemp;
        string weather = GameManager.Instance.savedWeather;

        temperatureText.text = "Temperature: " + temp.ToString("0") + "°C";
        weatherText.text = "Weather: " + weather;
    }
}

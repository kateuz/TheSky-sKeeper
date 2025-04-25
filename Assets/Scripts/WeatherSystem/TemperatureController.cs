using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    public float currentTemperature = 25f; // Starting temp
    public WeatherState weatherState;
    public WeatherEffects weatherEffects;

    void Start()
    {
        if (weatherState == null)
            weatherState = GetComponent<WeatherState>();

        UpdateWeatherBasedOnTemperature();
    }

    public void AdjustTemperature(float amount)
    {
        currentTemperature += amount;
        currentTemperature = Mathf.Clamp(currentTemperature, -20f, 100f); //limits

        UpdateWeatherBasedOnTemperature();
    }

    private void UpdateWeatherBasedOnTemperature()
    {
        if (currentTemperature < 10f)
        {
            weatherState.SetWeatherState(WeatherState.State.Cloudy);
            weatherEffects.SetWeatherEffect(WeatherState.State.Cloudy);

        }
        else if (currentTemperature < 25f)
        {
            weatherState.SetWeatherState(WeatherState.State.Rainy);
            weatherEffects.SetWeatherEffect(WeatherState.State.Rainy);
        }
        else if (currentTemperature < 40f)
        {
            weatherState.SetWeatherState(WeatherState.State.Sunny);
            weatherEffects.SetWeatherEffect(WeatherState.State.Sunny);
        }
        else
        {
            weatherState.SetWeatherState(WeatherState.State.Stormy);
            weatherEffects.SetWeatherEffect(WeatherState.State.Stormy);
        }
    }
}

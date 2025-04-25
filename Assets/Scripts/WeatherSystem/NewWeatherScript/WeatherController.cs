using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour {

    public float currentTemperature;

    public GameObject cloudy;
    public GameObject rainy;
    public GameObject sunny;
    public GameObject stormy;

    // Start is called before the first frame update
    void Start()
    {
        UpdateWeatherBasedOnTemperature();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustTemperature(float amount)
    {
        currentTemperature += amount;
        currentTemperature = Mathf.Clamp(currentTemperature, -20f, 100f); //limits

        UpdateWeatherBasedOnTemperature();
    }

    private void UpdateWeatherBasedOnTemperature()
    {
        if (currentTemperature <= 10f)
        {
            SetWeatherState("Cloudy");
        }
        else if (currentTemperature <= 25f)
        {
            SetWeatherState("Rainy");
        }
        else if (currentTemperature <= 40f)
        {
            SetWeatherState("Sunny");
        }
        else
        {
            SetWeatherState("Stormy");
        }
    }


    public void SetWeatherState(string weather)
    {
        string currentWeatherState = weather;


        if (currentWeatherState.Equals("Cloudy")) {
            sunny.SetActive(false);
            cloudy.SetActive(true);
            rainy.SetActive(false);
            stormy.SetActive(false);

        } else if (currentWeatherState.Equals("Rainy"))
        {
            sunny.SetActive(false);
            cloudy.SetActive(false);
            rainy.SetActive(true);
            stormy.SetActive(false);

        } else if (currentWeatherState.Equals("Sunny"))
        {
            sunny.SetActive(true);
            cloudy.SetActive(false);
            rainy.SetActive(false);
            stormy.SetActive(false);

        } else if (currentWeatherState.Equals("Stormy"))
        {
            sunny.SetActive(false);
            cloudy.SetActive(false);
            rainy.SetActive(false);
            stormy.SetActive(true);

        }


        //switch (currentWeatherState)
        //{
        //    case "Sunny":
        //        sunny.SetActive(true);
        //        Debug.Log("sunnyParticles PLAY");
        //        break;
        //    case "Cloudy":
        //        cloudy.SetActive(true);
        //        Debug.Log("cloudyParticles PLAY");
        //        break;
        //    case "Rainy":
        //        rainy.SetActive(true);
        //        Debug.Log("rainyParticles PLAY");
        //        break;
        //    case "Stormy":
        //        stormy.SetActive(true);
        //        Debug.Log("stormyParticles PLAY");
        //        break;
        //}
    }
}

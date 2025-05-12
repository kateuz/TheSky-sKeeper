using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoZone : MonoBehaviour
{

    public WeatherController weatherController;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            weatherController.isTornadoZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Killian"))
        {
            weatherController.isTornadoZone = false;
            weatherController.UpdateWeatherBasedOnTemperature();
        }
    }
}

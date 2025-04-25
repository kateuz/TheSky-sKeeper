using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherState : MonoBehaviour
{
    private int currentStateIndex;
    public static State currentWeatherState;
    public delegate void WeatherChangedHandler(State state);
    public event WeatherChangedHandler OnWeatherChanged;

    public ParticleSystem sunnyParticles;
    public ParticleSystem cloudyParticles;
    public ParticleSystem rainyParticles;
    public ParticleSystem stormyParticles;

    public enum State
    {
        Sunny, 
        Cloudy,
        Rainy, 
        Stormy,
    }

    public State[] WeatherStateOrder = new State[]
    {
        State.Sunny,
        State.Cloudy,
        State.Rainy,
        State.Stormy,
    };

    void Start()
    {
        currentStateIndex = 0;
        currentWeatherState = WeatherStateOrder[currentStateIndex];
    }

    public State GetCurrentWeatherState()
    {
        return WeatherStateOrder[currentStateIndex];
    }

    public void CycleWeatherState()
    {
        currentStateIndex = (currentStateIndex + 1) % WeatherStateOrder.Length;
        currentWeatherState = WeatherStateOrder[currentStateIndex];
        print("The weather is now: " + WeatherStateOrder[currentStateIndex]);

        OnWeatherChanged?.Invoke(currentWeatherState);
    }

    public int GetNextWeatherStateIndex()
    {
        return (currentStateIndex + 1) % WeatherStateOrder.Length;
    }

    public void SetWeatherState(State newState)
    {
        currentWeatherState = newState;
        currentStateIndex = System.Array.IndexOf(WeatherStateOrder, newState);
        print("Weather set to: " + newState);
        OnWeatherChanged?.Invoke(currentWeatherState);

        // Reset all particles
        //sunnyParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //cloudyParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //rainyParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //stormyParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        sunnyParticles.gameObject.SetActive(false);
        cloudyParticles.gameObject.SetActive(false);
        rainyParticles.gameObject.SetActive(false);
        stormyParticles.gameObject.SetActive(false);

        // Activate relevant one


        switch (newState)
        {
            case State.Sunny:
                sunnyParticles.Play();
                sunnyParticles.gameObject.SetActive(true);
                Debug.Log("sunnyParticles PLAY");
                break;
            case State.Cloudy:
                cloudyParticles.Play();
                cloudyParticles.gameObject.SetActive(true);
                Debug.Log("cloudyParticles PLAY");
                break;
            case State.Rainy:
                rainyParticles.Play();
                rainyParticles.gameObject.SetActive(true);
                Debug.Log("rainyParticles PLAY");
                break;
            case State.Stormy:
                stormyParticles.Play();
                stormyParticles.gameObject.SetActive(true);
                Debug.Log("stormyParticles PLAY");
                break;
        }
    }
}

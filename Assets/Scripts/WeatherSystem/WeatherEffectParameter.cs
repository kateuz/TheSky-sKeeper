using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class WeatherEffectParameter
{

    public Color cloudColor = Color.white;

    [Range(0, 1000)] public float cloudEmissionRate;
    [Range(0, 1000)] public float rainEmissionRate;
    [Range(0, 1000)] public float windSpeed;
    public bool sunRaysActive;
    //public bool cloudActive;
    //public bool rainActive;
    public bool lightningActive;
}

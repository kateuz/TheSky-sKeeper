using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem cloudParticles;

    public void SetCloudDarkness(Color color)
    {
        if (cloudParticles != null)
        {
            var main = cloudParticles.main;
            main.startColor = color;
        }
    }

    public void SetCloudEmissionRate(float rate)
    {
        if (cloudParticles != null)
        {
            var emission = cloudParticles.emission;
            emission.rateOverTime = rate;
        }
    }
}

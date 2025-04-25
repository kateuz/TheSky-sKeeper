using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem rainParticles;

    public void SetRainIntensity(float emissionRate, PlayerMovement playerMovement)
    {
        if (rainParticles != null)
        {
            var emission = rainParticles.emission;
            emission.rateOverTime = emissionRate;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnyEffect : MonoBehaviour
{
    [SerializeField] GameObject sunRays;

    public void ActivateSunnyEffect()
    {
        if (sunRays != null)
            sunRays.SetActive(true);
    }

    public void DeactivateSunnyEffect()
    {
        if (sunRays == null)
            sunRays.SetActive(false);
    }
}

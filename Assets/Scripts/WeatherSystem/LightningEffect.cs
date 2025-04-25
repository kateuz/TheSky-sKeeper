using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    [SerializeField] GameObject lightningFlash;

    public void ActivateLightningEffect()
    {
        if (lightningFlash != null)
            lightningFlash.SetActive(true);
    }

    public void DeactivateLightningEffect()
    {
        if (lightningFlash == null)
            lightningFlash.SetActive(false);
    }
}

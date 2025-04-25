using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffects : MonoBehaviour
{
    private WeatherEffectParameter currentWeatherEffectParameters;
    private WeatherEffectParameter targetWeatherEffectParameters;

    [SerializeField] WeatherEffectParameter sunnyWeatherParameters;
    [SerializeField] WeatherEffectParameter cloudyWeatherParameters;
    [SerializeField] WeatherEffectParameter rainyWeatherParameters;
    [SerializeField] WeatherEffectParameter stormyWeatherParameters;

    [SerializeField] SunnyEffect sunnyEffect;
    [SerializeField] CloudEffect cloudEffect;
    [SerializeField] RainEffect rainEffect;
    [SerializeField] LightningEffect lightningEffect;

    void Awake()
    {
        currentWeatherEffectParameters = sunnyWeatherParameters;
        targetWeatherEffectParameters = new WeatherEffectParameter();
    }

    void Start()
    {
        //SetWeatherEffect(WeatherState.State.Sunny);
    }

    public void SetWeatherEffect(WeatherState.State weatherState)
    {
        switch (weatherState)
        {
            case WeatherState.State.Sunny:
                targetWeatherEffectParameters = sunnyWeatherParameters;
                break;
            case WeatherState.State.Cloudy:
                targetWeatherEffectParameters = cloudyWeatherParameters;
                break;
            case WeatherState.State.Rainy:
                targetWeatherEffectParameters = rainyWeatherParameters;
                break;
            case WeatherState.State.Stormy:
                targetWeatherEffectParameters = stormyWeatherParameters;
                break;
        }
        StartCoroutine(TransitionToNextEffect());
    }

    private IEnumerator TransitionToNextEffect()
    {
        float transitionTime = 3f;
        float elapsedTime = 0;

        WeatherEffectParameter startWeatherEffectParameters = currentWeatherEffectParameters;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);

            currentWeatherEffectParameters = LerpWeatherEffectParameters(startWeatherEffectParameters, targetWeatherEffectParameters, t);
            UpdateWeatherEffects(currentWeatherEffectParameters, null);
            yield return null;
        }
        currentWeatherEffectParameters = targetWeatherEffectParameters;
        UpdateWeatherEffects(currentWeatherEffectParameters, null);
    }

    private WeatherEffectParameter LerpWeatherEffectParameters(WeatherEffectParameter from, WeatherEffectParameter to, float t)
    {
        WeatherEffectParameter result = new WeatherEffectParameter();
        result.cloudColor = Color.Lerp(from.cloudColor, to.cloudColor, t);
        result.cloudEmissionRate = Mathf.Lerp(from.cloudEmissionRate, to.cloudEmissionRate, t);
        result.rainEmissionRate = Mathf.Lerp(from.rainEmissionRate, to.rainEmissionRate, t);
        result.lightningActive = to.lightningActive;
        result.sunRaysActive = to.sunRaysActive;
        return result;
    }

    private void UpdateWeatherEffects(WeatherEffectParameter weatherEffectParameters, PlayerMovement playerMovement)
    {
        if (cloudEffect != null)
        {
            cloudEffect.SetCloudDarkness(weatherEffectParameters.cloudColor);
            cloudEffect.SetCloudEmissionRate(weatherEffectParameters.cloudEmissionRate);
        }
        if (rainEffect != null)
        {
            rainEffect.SetRainIntensity(weatherEffectParameters.rainEmissionRate, playerMovement);
        }

        if (lightningEffect != null)
        {
            if (weatherEffectParameters.lightningActive) lightningEffect.ActivateLightningEffect();
            else lightningEffect.DeactivateLightningEffect();
        }

        if (sunnyEffect != null)
        {
            if (weatherEffectParameters.sunRaysActive) sunnyEffect.ActivateSunnyEffect();
            else sunnyEffect.DeactivateSunnyEffect();
        }
    }

    //private void UpdateWeatherEffects(WeatherEffectParameter weatherEffectParameters, PlayerMovement playerMovement)
    //{
    //    cloudEffect.SetCloudDarkness(weatherEffectParameters.cloudColor);
    //    cloudEffect.SetCloudEmissionRate(weatherEffectParameters.cloudEmissionRate);
    //    rainEffect.SetRainIntensity(weatherEffectParameters.rainEmissionRate, playerMovement);

    //    if (weatherEffectParameters.lightningActive) lightningEffect.ActivateLightningEffect();
    //    else lightningEffect.DeactivateLightningEffect();

    //    if (weatherEffectParameters.sunRaysActive) sunnyEffect.ActivateSunnyEffect();
    //    else sunnyEffect.DeactivateSunnyEffect();
    //}

    public WeatherEffectParameter GetCurrentWeatherEffectParameters()
    {
        return currentWeatherEffectParameters;
    }
    public SunnyEffect GetSunnyEffect()
    {
        return sunnyEffect;
    }
    public CloudEffect GetCloudEffect()
    {
        return cloudEffect;
    }
    public RainEffect GetRainEffect()
    {
        return rainEffect;
    }
    public LightningEffect GetLightningEffect()
    {
        return lightningEffect;
    }
}


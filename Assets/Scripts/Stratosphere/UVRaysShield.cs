using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UVRaysShield : MonoBehaviour
{
    [SerializeField] private Light2D shieldLight;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private Color shieldColor = new Color(0.4f, 0.6f, 1f, 1f);

    private void Start()
    {
        if (shieldLight == null)
        {
            shieldLight = GetComponent<Light2D>();
        }

        if (shieldLight != null)
        {
            shieldLight.color = shieldColor;
            shieldLight.intensity = minIntensity;
        }
    }

    private void Update()
    {
        if (shieldLight != null && shieldLight.gameObject.activeSelf)
        {
            // Pulse the light intensity
            float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            shieldLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);
        }
    }
}

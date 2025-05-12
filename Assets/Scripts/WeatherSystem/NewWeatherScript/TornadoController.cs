using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class TornadoController : MonoBehaviour
{
    private bool playerInZone = false;

    public float tornadoStrength;
    public float maxStrength = 50f;
    public float minStrength = 0f;
    public float currentTemp;

    public float dmgAmount = 10f;
    public float dmgInterval = 1f; //seconds between each hit
    public float dmgTimer;

    public float safeTemp = 10f;

    public ParticleSystem tornado;
    public Collider2D tornadoCollider;
    void Start()
    {
        dmgTimer = 0f;
    }

    void Update()
    {

        if (playerInZone) {
        tornadoStrength = Mathf.Lerp(minStrength, maxStrength, Mathf.InverseLerp(-20, 100f, currentTemp));

        UpdateTornado();

            if (currentTemp <= safeTemp)
            {
                if (tornado.isPlaying)
                {
                    tornado.Stop();
                }

                if (tornadoCollider != null)
                {
                    tornadoCollider.enabled = false;
                }
            }
            else
            {
                if (!tornado.isPlaying)
                {
                    tornado.Play();
                }

                if (tornado != null)
                {
                    tornadoCollider.enabled = true;
                }
            }
        }
    }

    public void UpdateTornado()
    {
        var main = tornado.main;
        var emission = tornado.emission;
        var shape = tornado.shape;

        main.startSpeed = Mathf.Lerp(2f, 20f, tornadoStrength / maxStrength);

        shape.scale = new Vector3(
                Mathf.Lerp(1f, 5f, tornadoStrength / maxStrength), //width
                Mathf.Lerp(2f, 10f, tornadoStrength / maxStrength), //height
                Mathf.Lerp(2f, 5f, tornadoStrength / maxStrength) //depth
        );

        emission.rateOverTime = Mathf.Lerp(10f, 100f, tornadoStrength / maxStrength);
    }

    public void SetTemp(float temp)
    {
        currentTemp = temp;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            playerInZone = true;

            if (tornadoStrength >= 15)
            {
                dmgTimer += Time.deltaTime;

                if (dmgTimer >= dmgInterval)
                {
                    dmgTimer = 0f;

                    PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

                    if (playerHealth != null)
                    {
                        playerHealth.Damage(dmgAmount);
                    }
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            playerInZone = false;
            dmgTimer = 0f;
        }
    }
}

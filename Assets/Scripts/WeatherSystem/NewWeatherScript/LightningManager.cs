using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningManager : MonoBehaviour
{
    private PlayerHealth playerHealth;

    public GameObject player;
    public GameObject lightning;
    public float interval = 2f; //how often lightning strikes (secs)
    public float strikeRange = 10f;
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Killian");
        playerHealth = player.GetComponent<PlayerHealth>();

        InvokeRepeating("StrikeRandomly", 0f, interval);
    }
    void Update()
    {
        
    }

    void StrikeRandomly()
    {
        Vector3 randomPosition = player.transform.position + new Vector3(
                Random.Range(-5, 5), //x
                Random.Range(2f, 5f), //y 
                0f
            );

        float distance = Vector3.Distance(randomPosition, player.transform.position);

        if(distance < 2f)
        {
            GameObject newLightning = Instantiate(lightning, randomPosition, Quaternion.identity);
            Destroy(newLightning, 0.5f);

            PlayerStruck();
        }
        

        //CheckForPlayerStrike(randomPosition);
    }

    void CheckForPlayerStrike(Vector3 lightningPosition)
    {
        float distance = Vector3.Distance(lightningPosition, player.transform.position);

        if (distance < 2f) //adjust
        {
            PlayerStruck();
        }
    }

    void PlayerStruck()
    {
        if (playerHealth != null)
        {
            if (playerHealth.isProtected)
            {
                Debug.Log("Protektado ka dahil special ka");
            }
            else
            {
                playerHealth.Damage(30f); //dmg of lightning
                StartCoroutine(FlashRed());
            }
        }
    }

    IEnumerator FlashRed()
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
    }

}

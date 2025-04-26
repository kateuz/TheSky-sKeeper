using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI oxygenTxt;
    public GameObject questUI;
    //public GameObject oxygenText;

    public float oxygenLvl = 100f; //starting
    public float oxygenDepletionRate = 5f; //how fast oxygen drains
    public bool isDepleting = false;
    public bool hasOxygenMask = false;

    private PlayerHealth playerHealth;

    public float dmgRate = 10f;
    private float suffocationTimer;

    void Start()
    {
        if(questUI != null)
        {
            questUI.SetActive(false);
        }

        if (oxygenTxt != null) 
        {
            oxygenTxt.gameObject.SetActive(true);
        }

        playerHealth = FindObjectOfType<PlayerHealth>();

        isDepleting = true;
    }
    void Update()
    {
        if (isDepleting && !hasOxygenMask)
        {
            oxygenLvl -= oxygenDepletionRate * Time.deltaTime;
            oxygenLvl = Mathf.Clamp(oxygenLvl, 0, 100);

            if (oxygenTxt != null)
            {
                oxygenTxt.text = "Oxygen: " + Mathf.RoundToInt(oxygenLvl) + "%";
            }

            if (oxygenLvl <= 0)
            {
                Suffocate();
                Debug.Log("Suffocated..");
            }
        }
    }

    void Suffocate()
    {
        if ( playerHealth != null ) 
        {
            suffocationTimer += Time.deltaTime;
            if ( suffocationTimer >= 1f ) //damage every second
            {
                playerHealth.Damage(dmgRate);
                suffocationTimer = 0f;
            }
        }
    }

    public void StartDepleting()
    {
        isDepleting = true;
    }

    public void StopDepleting()
    {
        isDepleting = false;
    }

    public void EquipOxygenMask()
    {
        hasOxygenMask = true;
        Debug.Log("sumakses ka by the step by the step");

        //if( oxygenText != null )
        //{
        //    oxygenText.SetActive(true);
        //}
    }
}

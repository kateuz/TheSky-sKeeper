using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabConttroller : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    void Start()
    {
        ActivateTab(0);
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateTab(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && pages.Length > 1)
        {
            ActivateTab(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && pages.Length > 2)
        {
            ActivateTab(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && pages.Length > 3)
        {
            ActivateTab(3);
        }
    }

    public void ActivateTab(int tabNo)
    {
        if (tabNo >= 0 && tabNo < pages.Length)
        {
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].SetActive(false);
                tabImages[i].color = Color.grey;
            }
            pages[tabNo].SetActive(true);
            tabImages[tabNo].color = Color.white;
        }
        else
        {
            Debug.LogWarning("Invalid tab number: " + tabNo);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogueTrigger : MonoBehaviour
{
    public GameObject talkButton;
    public Dialogue dialogueSystem;

    void Start()
    {
        if (talkButton != null)
            talkButton.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            if (talkButton != null)
                talkButton.SetActive(true);

            // Connect the button click to starting dialogue
            talkButton.GetComponent<Button>().onClick.RemoveAllListeners();
            talkButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (dialogueSystem != null)
                {
                    dialogueSystem.StartDialogue();
                    talkButton.SetActive(false); // Hide after pressed
                }
            });
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian") && talkButton != null)
        {
            talkButton.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public Quest questToGive;
    private QuestManager questManager;

    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public GameObject continueButton;
    public Animator textDisplayAnim;
    public GameObject dialogueBox;

    private bool isTalking = false;

    public GameObject TrimmerPrefab;
    private void Start()
    {

        Debug.Log("Active: " + gameObject.activeInHierarchy);

        /* GameObject someInstance = Instantiate(TrimmerPrefab);
         Dialogue ss = someInstance.GetComponent<Dialogue>();
         ss.StartCoroutine(ss.CloseDialogue()); */

        questManager = FindObjectOfType<QuestManager>();

        dialogueBox.SetActive(false);

        continueButton.SetActive(false);
    }



    void Update()
    {
        if (textDisplay.text == sentences[index] && dialogueBox.activeSelf)
        {
            continueButton.SetActive(true);
        }
    }



    IEnumerator Type()
    {
        textDisplay.text = "";
        Debug.Log("Typing sentence: " + sentences[index]);
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        continueButton.SetActive(true);
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            continueButton.SetActive(false);
            StartCoroutine(Type());
        }
        else
        {
            GiveQuest();
            //FindObjectOfType<QuestManager>().StartDepleting();
            //Debug.Log("Oxygen decreasing..");

            StartCoroutine(CloseDialogue());
        }
    }

    void StartDialogue()
    {
        if (!isTalking)
        {
            isTalking = true;
            dialogueBox.SetActive(true);
            continueButton.SetActive(false);
            index = 0;
            StartCoroutine(Type());
        }
    }



    IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueBox.SetActive(false);
        isTalking = false;
        continueButton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Killian") && !isTalking)
        {
            StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Killian"))
        {
            StartCoroutine(CloseDialogue());
        }

    }

    public void GiveQuest()
    {
        if (questToGive != null)
        {
            questToGive.ActivateQuest();
        }
        else
        {
            Debug.LogWarning("questToGive is null!");
        }

        if (questManager != null)
        {
            Debug.Log("Quest Manager found!");

            if (questManager.questUI != null)
            {
                Debug.Log("Quest UI found, setting active!");
                questManager.questUI.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Quest UI is null!");
            }
        }
        else
        {
            Debug.LogWarning("Quest Manager is null!");
        }

        FindObjectOfType<QuestManager>().StartDepleting();
        Debug.Log("Quest Given!");
    }

    //    questToGive.ActivateQuest();

    //    if (questManager != null)
    //    {
    //        if (questManager.questUI != null)
    //            questManager.questUI.SetActive(true);

    //        if (questManager.oxygenTxt != null)
    //            questManager.oxygenTxt.gameObject.SetActive(true);
    //    }

    //    questManager.StartDepleting();
    //    Debug.Log("Quest Given!");
    //}
}
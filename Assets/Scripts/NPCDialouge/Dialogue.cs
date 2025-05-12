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
    public GameObject TrimmerPrefab;
    public GameObject dialogueBox;
    public GameObject leftMoveBtn;
    public GameObject rightMoveBtn;
    public GameObject jumpBtn;
    public GameObject scanBtn;
    public GameObject tempUpBtn;
    public GameObject tempDownBtn;
    public GameObject attackBtn;
    public GameObject interactButton;

    private bool isTalking = false;
    private bool isInRange = false;
    public TextMeshProUGUI continueBtnText;

    public Animator textDisplayAnim;

    private void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        dialogueBox.SetActive(false);
        continueButton.SetActive(false);
        if (interactButton != null)
        {
            interactButton.SetActive(false);
        }

        if (continueBtnText != null)
        {
            continueBtnText.text = "Next";
        }
        else
        {
            Debug.LogWarning("Continue button text component is missing!");
        }
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

            if (index == sentences.Length - 1 && continueBtnText != null)
            {
                continueBtnText.text = "Done";
            }

            StopAllCoroutines();
            StartCoroutine(Type());
        }
        else
        {
            GiveQuest();
            StartCoroutine(CloseDialogue());
        }
    }

    public void StartDialogue()
    {
        if (!isTalking)
        {
            isTalking = true;
            PlayerMovement.isDialogueActive = true;
            dialogueBox.SetActive(true);
            continueButton.SetActive(false);
            if (interactButton != null)
            {
                interactButton.SetActive(false);
            }
            index = 0;

            if (gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                StartCoroutine(Type());
            }
        }
    }

    private void SetButtonsActive(bool active)
    {
        if (leftMoveBtn != null) leftMoveBtn.SetActive(active);
        if (rightMoveBtn != null) rightMoveBtn.SetActive(active);
        if (jumpBtn != null) jumpBtn.SetActive(active);
        if (scanBtn != null) scanBtn.SetActive(active);
        if (tempUpBtn != null) tempUpBtn.SetActive(active);
        if (tempDownBtn != null) tempDownBtn.SetActive(active);
        if (attackBtn != null) attackBtn.SetActive(active);
    }

    IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(0.1f); // Small delay to ensure everything is processed
        
        dialogueBox.SetActive(false);
        continueButton.SetActive(false);
        isTalking = false;
        PlayerMovement.isDialogueActive = false;

        // Show interact button if player is still in range
        if (isInRange && interactButton != null)
        {
            interactButton.SetActive(true);
        }

        if (continueBtnText != null)
        {
            continueBtnText.text = "Next";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Killian"))
        {
            isInRange = true;
            if (!isTalking && interactButton != null)
            {
                interactButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Killian"))
        {
            isInRange = false;
            if (interactButton != null)
            {
                interactButton.SetActive(false);
            }
            if (isTalking)
            {
                StartCoroutine(CloseDialogue());
            }
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
            if (questManager.questUI != null)
            {
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
}
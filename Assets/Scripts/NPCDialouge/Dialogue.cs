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

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip talkingSound;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

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

        // Set up audio source
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("Added AudioSource component");
        }
    }

    void Update()
    {
        // Keep the continue button visible while dialogue is active
        if (dialogueBox.activeSelf)
        {
            continueButton.SetActive(true);
        }
    }

    IEnumerator Type()
    {
        textDisplay.text = "";
        Debug.Log($"Starting to type sentence {index}: {sentences[index]}");
        
        int letterCount = 0;
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            letterCount++;

            // Play sound every 3 letters
            if (letterCount % 3 == 0)
            {
                PlayTalkingSound();
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        Debug.Log($"Finished typing sentence {index}");
        continueButton.SetActive(true);
    }

    private void PlayTalkingSound()
    {
        if (talkingSound == null)
        {
            Debug.LogWarning("Talking sound is not assigned!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is missing!");
            return;
        }

        try
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(talkingSound, 0.2f);
            Debug.Log("Played talking sound");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error playing sound: {e.Message}");
        }
    }

    public void NextSentence()
    {
        Debug.Log($"NextSentence called. Current index: {index}, Total sentences: {sentences.Length}");
        
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            continueButton.SetActive(true);

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
            // Keep continue button visible from the start
            continueButton.SetActive(true);
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
        
        // Stop any playing sounds
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        
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
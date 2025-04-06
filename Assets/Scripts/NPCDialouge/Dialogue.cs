using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{

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

        Debug.Log("Active? " + gameObject.activeInHierarchy);

        /* GameObject someInstance = Instantiate(TrimmerPrefab);
         Dialogue ss = someInstance.GetComponent<Dialogue>();
         ss.StartCoroutine(ss.CloseDialogue()); */

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

}
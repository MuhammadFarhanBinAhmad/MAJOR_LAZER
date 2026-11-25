using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Button nextButton;

    [TextArea(3, 10)]
    public string[] dialogues;

    public float typingSpeed = 0.05f;

    private int currentDialogueIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    public string _nextSceneName;

    void Start()
    {
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (currentDialogueIndex >= dialogues.Length)
        {
            SceneManager.LoadScene(_nextSceneName);
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

        string currentMessage = dialogues[currentDialogueIndex];

        foreach (char letter in currentMessage)
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    public void NextDialogue()
    {
        // If text is still typing, show the full sentence first
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);

            dialogueText.text = dialogues[currentDialogueIndex];
            isTyping = false;
            typingCoroutine = null;
            return;
        }

        // Move to the next dialogue
        currentDialogueIndex++;

        ShowDialogue();
    }
}
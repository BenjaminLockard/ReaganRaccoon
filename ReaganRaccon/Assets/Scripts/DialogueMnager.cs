
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public Image npcPortraitImage;
    public Image reaganPortraitImage;

    [Header("Reagan")]
    public Sprite reaganPortrait;

    private DialogueLine[] currentLines;
    private int currentLineIndex;
    private bool dialogueActive;

    // Each line has a speaker, text, and portrait
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;

        [TextArea(2, 5)]
        public string dialogue;

        public Sprite speakerPortrait;
        public bool isReaganSpeaking;
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        if (dialogueActive || lines == null || lines.Length == 0)
            return;

        currentLines = lines;
        currentLineIndex = 0;
        dialogueActive = true;

        dialoguePanel.SetActive(true);
        ShowCurrentLine();
    }

    public void NextLine()
    {
        if (!dialogueActive)
            return;

        currentLineIndex++;

        if (currentLineIndex >= currentLines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = currentLines[currentLineIndex];

        speakerNameText.text = line.speakerName;
        dialogueText.text = line.dialogue;

        // Display NPC portrait
        if (npcPortraitImage != null)
        {
            npcPortraitImage.sprite =
                line.isReaganSpeaking ? null : line.speakerPortrait;

            npcPortraitImage.enabled =
                npcPortraitImage.sprite != null;
        }

        // Display Reagan portrait
        if (reaganPortraitImage != null)
        {
            reaganPortraitImage.sprite = reaganPortrait;
            reaganPortraitImage.enabled = reaganPortrait != null;

            // Highlight Reagan when she's speaking
            reaganPortraitImage.color = line.isReaganSpeaking
                ? Color.white
                : new Color(0.6f, 0.6f, 0.6f, 1f);
        }
    }

    public void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
        currentLines = null;
    }
}


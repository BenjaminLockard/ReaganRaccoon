
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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

    [Header("Portrait Appearance")]
    public Color speakingColor = Color.white;
    public Color dimmedColor =
        new Color(0.45f, 0.45f, 0.45f, 1f);

    [Header("Dialogue Choices")]
    public GameObject dialogueChoices;
    public Button choiceButton1;
    public Button choiceButton2;
    public TMP_Text continueHint;

    private DialogueLine[] currentLines;
    private int currentLineIndex;
    private bool dialogueActive;

    private Sprite currentNPCPortrait;
    private Sprite defaultNPCPortrait;
    private Sprite pendingNPCPortrait;
    private string currentNPCName;

    private bool showingChoices;
    private int selectedChoice;
    private bool showingChosenResponse;
    private bool showingNPCReply;
    private bool waitingForKeyRelease;

    private string pendingNPCReply;

    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;

        [TextArea(2, 5)]
        public string dialogue;

        public Sprite speakerPortrait;
        public bool isReaganSpeaking;

        [Header("Optional Reagan Choices")]
        public bool hasChoices;

        public string choice1 = "Uh, no...";
        public string choice2 = "Yeah, haha!";

        [TextArea(2, 5)]
        public string npcReply1;

        [TextArea(2, 5)]
        public string npcReply2;

        [Header("NPC Expressions After Choices")]
        public Sprite choice1ReplyPortrait;
        public Sprite choice2ReplyPortrait;
    }

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (choiceButton1 != null)
        {
            choiceButton1.onClick.AddListener(
                () => SelectChoice(0));
        }

        if (choiceButton2 != null)
        {
            choiceButton2.onClick.AddListener(
                () => SelectChoice(1));
        }
    }

    void Update()
    {
        if (!dialogueActive || !showingChoices)
            return;

        if (Keyboard.current == null)
            return;

        if (waitingForKeyRelease)
        {
            if (!Keyboard.current.eKey.isPressed)
                waitingForKeyRelease = false;

            return;
        }

        if (Keyboard.current.wKey.wasPressedThisFrame)
            ChangeSelection(-1);

        if (Keyboard.current.sKey.wasPressedThisFrame)
            ChangeSelection(1);

        if (Keyboard.current.eKey.wasPressedThisFrame)
            SelectChoice(selectedChoice);
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }

    public bool IsShowingChoices()
    {
        return showingChoices;
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        if (dialogueActive || lines == null ||
            lines.Length == 0)
            return;

        currentLines = lines;
        currentLineIndex = 0;
        dialogueActive = true;

        showingChoices = false;
        showingChosenResponse = false;
        showingNPCReply = false;
        waitingForKeyRelease = true;

        pendingNPCReply = null;
        pendingNPCPortrait = null;
        currentNPCPortrait = null;
        defaultNPCPortrait = null;
        currentNPCName = "NPC";

        foreach (DialogueLine line in currentLines)
        {
            if (!line.isReaganSpeaking)
            {
                currentNPCName = line.speakerName;

                if (line.speakerPortrait != null &&
                    defaultNPCPortrait == null)
                {
                    defaultNPCPortrait = line.speakerPortrait;
                }
            }
        }

        currentNPCPortrait = defaultNPCPortrait;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowCurrentLine();
    }

    public void NextLine()
    {
        if (!dialogueActive || showingChoices)
            return;

        currentLineIndex++;

        if (currentLineIndex >= currentLines.Length)
            EndDialogue();
        else
            ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = currentLines[currentLineIndex];

        // Reset expression for each new dialogue line.
        if (!line.isReaganSpeaking &&
            line.speakerPortrait != null)
        {
            currentNPCPortrait = line.speakerPortrait;
        }
        else
        {
            currentNPCPortrait = defaultNPCPortrait;
        }

        UpdatePortraits(line.isReaganSpeaking);

        if (dialogueText != null)
            dialogueText.text = line.dialogue;

        if (line.hasChoices)
        {
            ShowChoices(line);
        }
        else
        {
            showingChoices = false;

            if (dialogueChoices != null)
                dialogueChoices.SetActive(false);

            if (continueHint != null)
                continueHint.gameObject.SetActive(true);
        }
    }

    private void ShowChoices(DialogueLine line)
    {
        showingChoices = true;
        selectedChoice = 0;
        waitingForKeyRelease = true;

        if (dialogueChoices != null)
            dialogueChoices.SetActive(true);

        if (continueHint != null)
            continueHint.gameObject.SetActive(false);

        if (choiceButton1 != null)
        {
            TMP_Text text =
                choiceButton1.GetComponentInChildren<TMP_Text>();

            if (text != null)
                text.text = line.choice1;
        }

        if (choiceButton2 != null)
        {
            TMP_Text text =
                choiceButton2.GetComponentInChildren<TMP_Text>();

            if (text != null)
                text.text = line.choice2;
        }

        UpdateChoiceHighlight();
    }

    private void ChangeSelection(int direction)
    {
        selectedChoice += direction;

        if (selectedChoice < 0)
            selectedChoice = 1;

        if (selectedChoice > 1)
            selectedChoice = 0;

        UpdateChoiceHighlight();
    }

    private void UpdateChoiceHighlight()
    {
        if (choiceButton1 != null)
        {
            choiceButton1.image.color =
                selectedChoice == 0
                ? new Color(1f, 0.85f, 0.55f)
                : Color.white;
        }

        if (choiceButton2 != null)
        {
            choiceButton2.image.color =
                selectedChoice == 1
                ? new Color(1f, 0.85f, 0.55f)
                : Color.white;
        }
    }

    public void SelectChoice(int choiceIndex)
    {
        if (!dialogueActive || !showingChoices)
            return;

        DialogueLine line = currentLines[currentLineIndex];

        showingChoices = false;
        showingChosenResponse = true;

        if (dialogueChoices != null)
            dialogueChoices.SetActive(false);

        if (continueHint != null)
            continueHint.gameObject.SetActive(true);

        // Display Reagan's selected response.
        if (dialogueText != null)
        {
            dialogueText.text =
                choiceIndex == 0
                ? line.choice1
                : line.choice2;
        }

        UpdatePortraits(true);

        // Save NPC reply and matching facial expression.
        pendingNPCReply =
            choiceIndex == 0
            ? line.npcReply1
            : line.npcReply2;

        pendingNPCPortrait =
            choiceIndex == 0
            ? line.choice1ReplyPortrait
            : line.choice2ReplyPortrait;

        waitingForKeyRelease = true;
    }

    public void AdvanceDialogue()
    {
        if (!dialogueActive || showingChoices)
            return;

        if (showingChosenResponse)
        {
            showingChosenResponse = false;

            if (!string.IsNullOrEmpty(pendingNPCReply))
            {
                showingNPCReply = true;

                if (dialogueText != null)
                    dialogueText.text = pendingNPCReply;

                // Change NPC expression for their reply.
                currentNPCPortrait =
                    pendingNPCPortrait != null
                    ? pendingNPCPortrait
                    : defaultNPCPortrait;

                UpdatePortraits(false);
                return;
            }
        }
        else if (showingNPCReply)
        {
            showingNPCReply = false;
            pendingNPCReply = null;
            pendingNPCPortrait = null;
        }

        NextLine();
    }

    private void UpdatePortraits(bool isReaganSpeaking)
    {
        if (speakerNameText != null)
            speakerNameText.text = currentNPCName;

        if (npcPortraitImage != null)
        {
            npcPortraitImage.sprite = currentNPCPortrait;

            npcPortraitImage.enabled =
                currentNPCPortrait != null;

            npcPortraitImage.color =
                isReaganSpeaking
                ? dimmedColor
                : speakingColor;
        }

        if (reaganPortraitImage != null)
        {
            if (reaganPortrait != null)
                reaganPortraitImage.sprite = reaganPortrait;

            reaganPortraitImage.enabled =
                reaganPortraitImage.sprite != null;

            reaganPortraitImage.color =
                isReaganSpeaking
                ? speakingColor
                : dimmedColor;
        }
    }

    public void EndDialogue()
    {
        dialogueActive = false;
        showingChoices = false;
        showingChosenResponse = false;
        showingNPCReply = false;

        pendingNPCReply = null;
        pendingNPCPortrait = null;
        currentLines = null;
        currentNPCPortrait = null;
        defaultNPCPortrait = null;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}




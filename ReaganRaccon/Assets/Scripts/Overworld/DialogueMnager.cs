
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

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
    public Button choiceButton3;
    public TMP_Text continueHint;

    [Header("Typewriter")]
    public float typeSpeed = 0.035f;

    [Header("Ranger Rejection")]
    public int badChoicesToReject = 3;

    [TextArea(2, 5)]
    public string rejectionDialogue =
        "That's three red flags! " +
        "You're not coming into these campgrounds!";

    public Sprite rejectionPortrait;

    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;

        [TextArea(2, 5)]
        public string dialogue;

        public Sprite speakerPortrait;
        public bool isReaganSpeaking;

        [Header("Choices")]
        public bool hasChoices;

        public string choice1;
        public string choice2;
        public string choice3;

        [TextArea(2, 5)]
        public string npcReply1;

        [TextArea(2, 5)]
        public string npcReply2;

        [TextArea(2, 5)]
        public string npcReply3;

        [Header("Reply Expressions")]
        public Sprite choice1ReplyPortrait;
        public Sprite choice2ReplyPortrait;
        public Sprite choice3ReplyPortrait;

        [Header("Future Overworld Time Changes")]
        public float choice1TimeChange;
        public float choice2TimeChange;
        public float choice3TimeChange;

        [Header("Bad Choices")]
        public bool choice1IsBad;
        public bool choice2IsBad;
        public bool choice3IsBad;

        [Header("Branching: Next Element Index")]
        [Tooltip("-2 = next element, -1 = end conversation")]
        public int choice1NextLine = -2;
        public int choice2NextLine = -2;
        public int choice3NextLine = -2;
    }

    private DialogueLine[] currentLines;
    private int currentLineIndex;
    private int pendingNextLine;

    private bool dialogueActive;
    private bool showingChoices;
    private bool choicesPending;
    private bool showingChosenResponse;
    private bool showingNPCReply;
    private bool showingRejection;
    private bool waitingForKeyRelease;

    private int selectedChoice;
    private int lastChoiceFrame = -1;

    private int consecutiveBadChoices;
    private bool entryDenied;

    private string currentNPCName;
    private string pendingNPCReply;

    private Sprite defaultNPCPortrait;
    private Sprite currentNPCPortrait;
    private Sprite pendingNPCPortrait;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private string fullDialogueText = "";

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (choiceButton1 != null)
            choiceButton1.onClick.AddListener(
                () => SelectChoice(0));

        if (choiceButton2 != null)
            choiceButton2.onClick.AddListener(
                () => SelectChoice(1));

        if (choiceButton3 != null)
            choiceButton3.onClick.AddListener(
                () => SelectChoice(2));
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
        // Prevent NPCDialogue from advancing while
        // a question is still typing.
        return showingChoices || choicesPending;
    }

    public bool IsEntryDenied()
    {
        return entryDenied;
    }

    public int GetBadChoiceStreak()
    {
        return consecutiveBadChoices;
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        if (dialogueActive ||
            lines == null ||
            lines.Length == 0)
            return;

        currentLines = lines;
        currentLineIndex = 0;
        pendingNextLine = 0;

        dialogueActive = true;
        showingChoices = false;
        choicesPending = false;
        showingChosenResponse = false;
        showingNPCReply = false;
        showingRejection = false;

        waitingForKeyRelease = true;
        lastChoiceFrame = -1;

        pendingNPCReply = null;
        pendingNPCPortrait = null;

        consecutiveBadChoices = 0;
        currentNPCName = "NPC";
        defaultNPCPortrait = null;

        foreach (DialogueLine line in lines)
        {
            if (!line.isReaganSpeaking)
            {
                currentNPCName = line.speakerName;

                if (defaultNPCPortrait == null &&
                    line.speakerPortrait != null)
                {
                    defaultNPCPortrait =
                        line.speakerPortrait;
                }
            }
        }

        currentNPCPortrait = defaultNPCPortrait;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (entryDenied)
            ShowRejection();
        else
            ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = currentLines[currentLineIndex];

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

        showingChoices = false;
        choicesPending = line.hasChoices;

        if (dialogueChoices != null)
            dialogueChoices.SetActive(false);

        if (continueHint != null)
            continueHint.gameObject.SetActive(
                !line.hasChoices);

        DisplayDialogue(line.dialogue);
    }

    IEnumerator Type(
        TMP_Text dest,
        string text,
        float speed)
    {
        isTyping = true;
        dest.text = "";

        foreach (char letter in text)
        {
            dest.text += letter;

            yield return new WaitForSeconds(
                Mathf.Max(0.001f, speed));
        }

        isTyping = false;
        typingCoroutine = null;

        OnTypingFinished();
    }

    private void DisplayDialogue(string text)
    {
        if (dialogueText == null)
            return;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        fullDialogueText = text ?? "";
        isTyping = false;

        typingCoroutine = StartCoroutine(
            Type(dialogueText, fullDialogueText, typeSpeed));
    }

    private void FinishTyping()
    {
        if (!isTyping)
            return;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;

        if (dialogueText != null)
            dialogueText.text = fullDialogueText;

        OnTypingFinished();
    }

    private void OnTypingFinished()
    {
        if (dialogueActive && choicesPending)
        {
            choicesPending = false;
            ShowChoices(currentLines[currentLineIndex]);
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

        SetButtonText(choiceButton1, line.choice1);
        SetButtonText(choiceButton2, line.choice2);
        SetButtonText(choiceButton3, line.choice3);

        UpdateChoiceHighlight();
    }

    private void SetButtonText(Button button, string text)
    {
        if (button == null)
            return;

        TMP_Text label =
            button.GetComponentInChildren<TMP_Text>();

        if (label != null)
            label.text = text;
    }

    private void ChangeSelection(int direction)
    {
        selectedChoice =
            (selectedChoice + direction + 3) % 3;

        UpdateChoiceHighlight();
    }

    private void UpdateChoiceHighlight()
    {
        Highlight(choiceButton1, selectedChoice == 0);
        Highlight(choiceButton2, selectedChoice == 1);
        Highlight(choiceButton3, selectedChoice == 2);
    }

    private void Highlight(Button button, bool selected)
    {
        if (button == null || button.image == null)
            return;

        button.image.color = selected
            ? new Color(1f, 0.85f, 0.55f)
            : Color.white;
    }

    public void SelectChoice(int choiceIndex)
    {
        if (!dialogueActive ||
            !showingChoices ||
            isTyping)
            return;

        if (choiceIndex < 0 || choiceIndex > 2)
            return;

        DialogueLine line = currentLines[currentLineIndex];

        string chosenText;
        bool isBad;
        float timeChange;
        int nextLine;

        switch (choiceIndex)
        {
            case 0:
                chosenText = line.choice1;
                pendingNPCReply = line.npcReply1;
                pendingNPCPortrait =
                    line.choice1ReplyPortrait;
                isBad = line.choice1IsBad;
                timeChange = line.choice1TimeChange;
                nextLine = line.choice1NextLine;
                break;

            case 1:
                chosenText = line.choice2;
                pendingNPCReply = line.npcReply2;
                pendingNPCPortrait =
                    line.choice2ReplyPortrait;
                isBad = line.choice2IsBad;
                timeChange = line.choice2TimeChange;
                nextLine = line.choice2NextLine;
                break;

            default:
                chosenText = line.choice3;
                pendingNPCReply = line.npcReply3;
                pendingNPCPortrait =
                    line.choice3ReplyPortrait;
                isBad = line.choice3IsBad;
                timeChange = line.choice3TimeChange;
                nextLine = line.choice3NextLine;
                break;
        }

        // -2 means continue to the next element.
        pendingNextLine = nextLine == -2
            ? currentLineIndex + 1
            : nextLine;

        showingChoices = false;
        showingChosenResponse = true;
        lastChoiceFrame = Time.frameCount;

        if (dialogueChoices != null)
            dialogueChoices.SetActive(false);

        if (continueHint != null)
            continueHint.gameObject.SetActive(true);

        UpdatePortraits(true);
        DisplayDialogue(chosenText);

        if (isBad)
            consecutiveBadChoices++;
        else
            consecutiveBadChoices = 0;

        if (consecutiveBadChoices >=
            Mathf.Max(1, badChoicesToReject))
        {
            entryDenied = true;
        }

        // Timer adjustments are NOT applied yet.
        Debug.Log(
            "Time adjustment reserved: " + timeChange +
            " | Bad streak: " + consecutiveBadChoices +
            " | Next line: " + pendingNextLine);
    }

    public void AdvanceDialogue()
    {
        if (!dialogueActive || showingChoices)
            return;

        if (Time.frameCount == lastChoiceFrame)
            return;

        if (isTyping)
        {
            FinishTyping();
            return;
        }

        if (choicesPending)
            return;

        if (showingRejection)
        {
            EndDialogue();
            return;
        }

        if (showingChosenResponse)
        {
            showingChosenResponse = false;

            if (entryDenied)
            {
                ShowRejection();
                return;
            }

            if (!string.IsNullOrEmpty(pendingNPCReply))
            {
                showingNPCReply = true;

                currentNPCPortrait =
                    pendingNPCPortrait != null
                    ? pendingNPCPortrait
                    : defaultNPCPortrait;

                UpdatePortraits(false);
                DisplayDialogue(pendingNPCReply);
                return;
            }

            GoToLine(pendingNextLine);
            return;
        }

        if (showingNPCReply)
        {
            showingNPCReply = false;
            pendingNPCReply = null;
            pendingNPCPortrait = null;

            GoToLine(pendingNextLine);
            return;
        }

        GoToLine(currentLineIndex + 1);
    }

    private void GoToLine(int index)
    {
        if (index == -1 ||
            index >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        if (index < 0)
        {
            Debug.LogWarning(
                "Invalid dialogue element: " + index);
            EndDialogue();
            return;
        }

        currentLineIndex = index;
        ShowCurrentLine();
    }

    public void NextLine()
    {
        if (!dialogueActive ||
            showingChoices ||
            choicesPending)
            return;

        GoToLine(currentLineIndex + 1);
    }

    private void ShowRejection()
    {
        showingChoices = false;
        choicesPending = false;
        showingChosenResponse = false;
        showingNPCReply = false;
        showingRejection = true;

        if (dialogueChoices != null)
            dialogueChoices.SetActive(false);

        if (continueHint != null)
            continueHint.gameObject.SetActive(true);

        currentNPCPortrait = rejectionPortrait != null
            ? rejectionPortrait
            : defaultNPCPortrait;

        UpdatePortraits(false);
        DisplayDialogue(rejectionDialogue);

        Debug.Log("CAMPGROUND ENTRY DENIED!");
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

            npcPortraitImage.color = isReaganSpeaking
                ? dimmedColor
                : speakingColor;
        }

        if (reaganPortraitImage != null)
        {
            if (reaganPortrait != null)
                reaganPortraitImage.sprite = reaganPortrait;

            reaganPortraitImage.enabled =
                reaganPortraitImage.sprite != null;

            reaganPortraitImage.color = isReaganSpeaking
                ? speakingColor
                : dimmedColor;
        }
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;
        dialogueActive = false;
        showingChoices = false;
        choicesPending = false;
        showingChosenResponse = false;
        showingNPCReply = false;
        showingRejection = false;

        pendingNPCReply = null;
        pendingNPCPortrait = null;
        currentLines = null;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}






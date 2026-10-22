
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    [Header("Reward Window")]
    public GameObject rewardWindow;

    [Header("Lose Window")]
    public GameObject loseWindow;

    [Header("Player Controls")]
    public PlayerInput overworldInput;

    [Header("Random Rewards")]
    public RewardItem[] possibleRewards;

    public TMP_Text itemNameText;
    public TMP_Text itemValueText;
    public TMP_Text totalMoneyText;
    public Image itemImageDisplay;

    [Header("Money Management")]
    public MoneyManager moneyManager;

    private float windowOpenedTime;
    private bool resultWindowOpen = false;

    [System.Serializable]
    public class RewardItem
    {
        public string itemName;
        public Sprite itemImage;
        public float itemValue;
    }

    void Start()
    {
        if (rewardWindow != null)
            rewardWindow.SetActive(false);

        if (loseWindow != null)
            loseWindow.SetActive(false);
    }

    void Update()
    {
        if (!resultWindowOpen)
            return;

        // Prevent the final minigame input from
        // immediately closing the result window.
        if (Time.unscaledTime - windowOpenedTime < 0.25f)
            return;

        bool keyboardPressed = false;
        bool mouseClicked = false;

        if (Keyboard.current != null)
        {
            keyboardPressed =
                Keyboard.current.wKey.wasPressedThisFrame ||
                Keyboard.current.aKey.wasPressedThisFrame ||
                Keyboard.current.sKey.wasPressedThisFrame ||
                Keyboard.current.dKey.wasPressedThisFrame ||
                Keyboard.current.spaceKey.wasPressedThisFrame;
        }

        if (Mouse.current != null)
        {
            mouseClicked =
                Mouse.current.leftButton.wasPressedThisFrame ||
                Mouse.current.rightButton.wasPressedThisFrame ||
                Mouse.current.middleButton.wasPressedThisFrame;
        }

        if (keyboardPressed || mouseClicked)
            ContinueToMainGame();
    }

    // Called when the player WINS a minigame.
    public void ShowRewardWindow()
    {
        if (resultWindowOpen)
            return;

        GameManager.Instance.setRootTrue();

        if (overworldInput != null)
            overworldInput.enabled = false;

        if (loseWindow != null)
            loseWindow.SetActive(false);

        ChooseRandomReward();

        if (rewardWindow != null)
            rewardWindow.SetActive(true);

        windowOpenedTime = Time.unscaledTime;
        resultWindowOpen = true;

        Debug.Log("MINIGAME WON!");
    }

    // Called when the player LOSES a minigame.
    public void ShowLoseWindow()
    {
        if (resultWindowOpen)
            return;

        GameManager.Instance.setRootTrue();

        if (overworldInput != null)
            overworldInput.enabled = false;

        if (rewardWindow != null)
            rewardWindow.SetActive(false);

        // No random item is selected.
        // No money is added.
        if (loseWindow != null)
            loseWindow.SetActive(true);

        windowOpenedTime = Time.unscaledTime;
        resultWindowOpen = true;

        Debug.Log("MINIGAME LOST! No reward awarded.");
    }

    private void ChooseRandomReward()
    {
        if (possibleRewards == null ||
            possibleRewards.Length == 0)
        {
            Debug.LogWarning("No reward items assigned!");
            return;
        }

        int randomIndex =
            Random.Range(0, possibleRewards.Length);

        RewardItem chosenItem =
            possibleRewards[randomIndex];

        if (itemNameText != null)
        {
            itemNameText.text =
                "You Won <b>" + chosenItem.itemName + "</b>!";
        }

        if (moneyManager != null)
        {
            moneyManager.changeMoney(chosenItem.itemValue);

            if (totalMoneyText != null)
            {
                totalMoneyText.text =
                    "Total Money: $" +
                    moneyManager.getMoney().ToString("F2");
            }
        }

        if (itemValueText != null)
        {
            itemValueText.text =
                "Money Earned: $" +
                chosenItem.itemValue.ToString("F2");
        }

        if (itemImageDisplay != null)
        {
            itemImageDisplay.sprite = chosenItem.itemImage;
            itemImageDisplay.enabled =
                chosenItem.itemImage != null;
        }

        Debug.Log("Player won: " + chosenItem.itemName);
    }

    // Closes either result window.
    public void ContinueToMainGame()
    {
        if (!resultWindowOpen)
            return;

        resultWindowOpen = false;

        if (rewardWindow != null)
            rewardWindow.SetActive(false);

        if (loseWindow != null)
            loseWindow.SetActive(false);

        if (overworldInput != null)
            overworldInput.enabled = true;

        Debug.Log("RESULT WINDOW CLOSED!");
    }
}



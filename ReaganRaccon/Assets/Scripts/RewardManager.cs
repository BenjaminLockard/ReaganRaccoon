
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    [Header("Reward Window")]
    public GameObject rewardWindow;

    [Header("Player Controls")]
    public PlayerInput overworldInput;

    [Header("Random Rewards")]
    public RewardItem[] possibleRewards;

    public TMP_Text itemNameText;
    public TMP_Text itemValueText;
    public TMP_Text totalMoneyText;
    public Image itemImageDisplay;

    private float rewardOpenedTime;
    private bool rewardOpen = false;
    //FOR MONEY MANAGEMENT
    public MoneyManager moneyManager;

    // Stores each reward's name and image
    [System.Serializable]
    public class RewardItem
    {
        public string itemName;
        public Sprite itemImage;

        public float itemValue;
    }

    void Update()
    {
        // Only check input when reward window is open
        if (!rewardOpen)
            return;

        // Prevent the last minigame input
        // from immediately closing the reward screen
        if (Time.unscaledTime - rewardOpenedTime < 0.25f)
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
        {
            ContinueToMainGame();
        }
    }

    // Called when a minigame finishes
    public void ShowRewardWindow()
    {
        // Show the overworld and main camera
        GameManager.Instance.setRootTrue();

        // Temporarily disable Reagan's controls
        if (overworldInput != null)
            overworldInput.enabled = false;

        // Choose a random reward
        ChooseRandomReward();

        // Display the reward window
        rewardWindow.SetActive(true);

        rewardOpenedTime = Time.unscaledTime;
        rewardOpen = true;
    }

    // Randomly selects an item
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

        // Display the item name with "You Won"
        if (itemNameText != null)
        {
           itemNameText.text =
              "You Won <b>" + chosenItem.itemName + "</b>!";
            moneyManager.changeMoney(chosenItem.itemValue);
            itemValueText.text = "Money Earned: $" + (chosenItem.itemValue.ToString("F2"));
            totalMoneyText.text = "Total Money: $" + moneyManager.getMoney().ToString("F2");
        }

        // Display the item's artwork
        if (itemImageDisplay != null)
        {
            itemImageDisplay.sprite = chosenItem.itemImage;

            // Hide image if artwork isn't assigned yet
            itemImageDisplay.enabled =
                chosenItem.itemImage != null;
        }

        Debug.Log("Player won: " + chosenItem.itemName);
    }

    // Called by WASD, Space, or mouse click
    public void ContinueToMainGame()
    {
        if (!rewardOpen)
            return;

        rewardOpen = false;

        // Hide reward window
        rewardWindow.SetActive(false);

        // Restore Reagan's controls
        if (overworldInput != null)
            overworldInput.enabled = true;

        Debug.Log("REWARD SCREEN CLOSED!");
    }
}


using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardWindow : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text moneyEarnedText;
    public TMP_Text totalMoneyText;
    public Image itemImage;

    public void ShowReward(string itemName, int moneyEarned, int totalMoney, Sprite itemSprite)
    {
        itemNameText.text = "Item Won: " + itemName;
        moneyEarnedText.text = "Money Earned: $" + moneyEarned;
        totalMoneyText.text = "Total Money: $" + totalMoney;

        itemImage.sprite = itemSprite;

        gameObject.SetActive(true);
    }

    public void CloseReward()
    {
        gameObject.SetActive(false);
    }
}
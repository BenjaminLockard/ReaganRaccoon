using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private float currentMoney;
    public TMP_Text moneyText;

    public void changeMoney(float change)
    {
        currentMoney += change;
        moneyText.text = "$" + currentMoney.ToString("F2");
    }

    void Start()
    {
        currentMoney = 0.00f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

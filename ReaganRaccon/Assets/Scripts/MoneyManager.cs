using TMPro;
using UnityEngine;
using System.Collections;

public class MoneyManager : MonoBehaviour
{
    private float currentMoney, goingSum;
    public TMP_Text moneyText, updateText;

    public float getCurrentMoney()
    {
        return currentMoney;
    }

    public void changeMoney(float amount)
    {
        if (amount == 0f)
            return;
        
        currentMoney += amount;
        goingSum += amount;
        moneyText.text = "$" + currentMoney.ToString("F2");

        if (amount > 0)
        {
            updateText.text = "+" + goingSum.ToString("F2");
            updateText.color = new Color(0.5f, 1f, 0.5f, 1f);
        }
        else
        {
            updateText.text = goingSum.ToString("F2");
            updateText.color = new Color(1f, 0f, 0f, 1f);
        }

        StartCoroutine(changeFeedback());
    }

    public IEnumerator changeFeedback()
    {
        yield return new WaitForSeconds(1.5f);
        
        updateText.color = new Color(0f, 0f, 0f, 0f);
        goingSum = 0f;
    }

    public float getMoney()
    {
        return currentMoney;
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

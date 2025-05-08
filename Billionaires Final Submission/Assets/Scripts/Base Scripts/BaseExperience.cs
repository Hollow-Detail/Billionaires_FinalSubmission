using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseExperience : MonoBehaviour
{
    public teamColor.ColorChoice baseTeam;
    public Image expRadial;         // Assign radial UI Image (type: Filled, Radial 360)
    public TextMeshProUGUI rankText; // Assign TMP text placed in center of base

    int currentXP = 0;
    int currentRank = 1;
    int xpThreshold = 10; // Starting threshold

    void Start()
    {
        ExperienceObserver.Instance.RegisterBase(this);
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= xpThreshold)
        {
            currentXP -= xpThreshold;
            currentRank++;
            xpThreshold = Mathf.CeilToInt(xpThreshold * 1.5f); // or use *2f for stricter scaling
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        expRadial.fillAmount = (float)currentXP / xpThreshold;
        rankText.text = currentRank.ToString();
    }
}

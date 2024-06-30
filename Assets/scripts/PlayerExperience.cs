using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    public int level = 1;
    public int experiencePoints = 0;
    public int experienceToNextLevel = 20;

    public Slider experienceSlider;
    public TMP_Text levelText;
    public UpgradeUI upgradeUI;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        UpdateUI();
    }

    public void AddExperience(int amount)
    {
        experiencePoints += amount;
        if (experiencePoints >= experienceToNextLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }

    void LevelUp()
    {
        level++;
        experiencePoints = 0;
        experienceToNextLevel *= 2;
        UpdateUI();
        playerHealth.RefillHealth();
        upgradeUI.ShowUpgradePanel();
    }

    void UpdateUI()
    {
        if (experienceSlider != null)
        {
            experienceSlider.maxValue = experienceToNextLevel;
            experienceSlider.value = experiencePoints;
        }

        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
    }
}

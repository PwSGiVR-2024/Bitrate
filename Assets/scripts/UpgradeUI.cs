using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;

    public Button upgradeButtonLeft;
    public Image upgradeIconLeft;
    public TMP_Text upgradeNameLeft;
    public TMP_Text upgradeDescriptionLeft;

    public Button upgradeButtonRight;
    public Image upgradeIconRight;
    public TMP_Text upgradeNameRight;
    public TMP_Text upgradeDescriptionRight;

    private UpgradeManager upgradeManager;
    private Upgrade[] currentUpgrades;

    void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
        HideUpgradePanel();
    }

    public void ShowUpgradePanel()
    {
        Time.timeScale = 0f;
        upgradePanel.SetActive(true);
        currentUpgrades = upgradeManager.GetRandomUpgrades(2);

        SetUpgradeOption(currentUpgrades[0], upgradeButtonLeft, upgradeIconLeft, upgradeNameLeft, upgradeDescriptionLeft);
        SetUpgradeOption(currentUpgrades[1], upgradeButtonRight, upgradeIconRight, upgradeNameRight, upgradeDescriptionRight);
    }

    void SetUpgradeOption(Upgrade upgrade, Button button, Image icon, TMP_Text name, TMP_Text description)
    {
        icon.sprite = upgrade.icon;
        name.text = upgrade.name;
        description.text = upgrade.description;
        button.onClick.AddListener(() => SelectUpgrade(upgrade));
    }

    public void HideUpgradePanel()
    {
        Time.timeScale = 1f;
        upgradePanel.SetActive(false);

        upgradeButtonLeft.onClick.RemoveAllListeners();
        upgradeButtonRight.onClick.RemoveAllListeners();
    }

    public void SelectUpgrade(Upgrade upgrade)
    {
        upgrade.onApplyUpgrade.Invoke();
        upgradeManager.RemoveUpgradesFromPool(currentUpgrades);
        HideUpgradePanel();
    }
}

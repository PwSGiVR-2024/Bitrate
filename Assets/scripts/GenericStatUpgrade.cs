using UnityEngine;

[CreateAssetMenu(fileName = "GenericStatUpgrade", menuName = "Upgrades/GenericStatUpgrade")]
public class GenericStatUpgrade : ScriptableObject
{
    public string[] upgradeTypes; // Types of upgrades (e.g., "Speed", "Health", "RateOfFire")
    public float[] upgradeValues; // Corresponding values for each upgrade type

    public void ApplyUpgrade()
    {
        UpgradeManager upgradeManager = FindObjectOfType<UpgradeManager>();
        for (int i = 0; i < upgradeTypes.Length; i++)
        {
            upgradeManager.ApplyUpgrade(upgradeTypes[i], upgradeValues[i]);
        }
    }
}

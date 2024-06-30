using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> upgradePool = new List<Upgrade>();
    public Upgrade genericStatUpgrade;

    private List<Upgrade> remainingUpgrades;

    void Start()
    {
        remainingUpgrades = new List<Upgrade>(upgradePool);
    }

    public Upgrade[] GetRandomUpgrades(int count)
    {
        List<Upgrade> selectedUpgrades = new List<Upgrade>();

        for (int i = 0; i < count; i++)
        {
            if (remainingUpgrades.Count > 0)
            {
                int randomIndex = Random.Range(0, remainingUpgrades.Count);
                selectedUpgrades.Add(remainingUpgrades[randomIndex]);
                remainingUpgrades.RemoveAt(randomIndex);
            }
            else
            {
                selectedUpgrades.Add(genericStatUpgrade);
            }
        }

        return selectedUpgrades.ToArray();
    }

    public void RemoveUpgradesFromPool(Upgrade[] upgrades)
    {
        foreach (var upgrade in upgrades)
        {
            remainingUpgrades.Remove(upgrade);
        }
    }

    public void ApplyUpgrade(string upgradeType, float value)
    {
        IUpgradeable[] upgradeableComponents = FindObjectsOfType<MonoBehaviour>().OfType<IUpgradeable>().ToArray();
        foreach (var component in upgradeableComponents)
        {
            component.ApplyUpgrade(upgradeType, value);
        }
    }
}

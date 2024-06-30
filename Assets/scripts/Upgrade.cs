using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string name;
    public string description;
    public Sprite icon;
    public UnityEvent onApplyUpgrade;
}

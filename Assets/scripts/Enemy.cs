using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int experiencePoints = 1; // Experience points this enemy is worth

    private void OnDestroy()
    {
        DropExperienceOrbs();
    }

    private void DropExperienceOrbs()
    {
        // Assuming you have an ExperienceOrb prefab
        GameObject experienceOrbPrefab = Resources.Load<GameObject>("ExperienceOrb");

        // Instantiate the orb at the enemy's position
        Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
    }
}

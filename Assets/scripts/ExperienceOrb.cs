using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    public float attractionRadius = 3f;
    public float attractionSpeed = 5f;

    private Transform player;
    private bool isAttracted = false;
    private int experienceValue = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < attractionRadius)
        {
            isAttracted = true;
        }

        if (isAttracted)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, attractionSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerExperience playerExperience = other.GetComponent<PlayerExperience>();
            if (playerExperience != null)
            {
                playerExperience.AddExperience(experienceValue);
            }
            Destroy(gameObject);
        }
    }

    public void SetExperienceValue(int value)
    {
        experienceValue = value;
        float sizeMultiplier = Mathf.Sqrt(value);
        // Orb gets bigger the more experience it gives. 
        transform.localScale = new Vector3(sizeMultiplier / 10f + 0.2f, sizeMultiplier / 10f + 0.2f, sizeMultiplier / 10f + 0.2f);
    }
}

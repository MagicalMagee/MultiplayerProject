using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth = 100; // Max Health
    public bool godMode = false; // Flag to indicate whether god mode is enabled

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (!godMode) // Check if god mode is not enabled
        {
            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
            }
            
            // Damage Health
            health -= amount;

            if (health <= 0)
            {
                // Die when at 0 HP
                Die();
            }
        }
    }

    // Healing factor when steps on "Stars" Object
    public void Heal(int amount)
    {
        if (!godMode) // Check if god mode is not enabled
        {
            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
            }

            bool wouldBeOverMaxHealth = health + amount > maxHealth;

            // No Overhealing
            if (wouldBeOverMaxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += amount;
            }
        }
    }

    private void Die()
    {
        // Death
        Destroy(gameObject);
    }

    // Toggle god mode on/off in Unity
    public void ToggleGodMode(bool enabled)
    {
        godMode = enabled;
    }

    // Collision detection with "Star" objects
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Star"))
        {
            // Increase player's health, +10 HP for Players
            Heal(10);
        }
    }
}

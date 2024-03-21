using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
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

            health -= amount;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int amount)
    {
        if (!godMode) // Check if god mode is not enabled
        {
            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
            }

            bool wouldBeOverMaxHealth = health + amount > maxHealth;

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
        Debug.Log("I am Dead!");
        Destroy(gameObject);
    }

    // Method to toggle god mode on/off
    public void ToggleGodMode(bool enabled)
    {
        godMode = enabled;
    }

    // Collision detection with "Star" objects
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Star"))
        {
            // Increase player's health
            Heal(10); // Assuming healing the player by 10 when colliding with a star
            
            // Debug message
            Debug.Log("Player collided with a star! Health increased by 10. Current health: " + health);
        }
    }
}

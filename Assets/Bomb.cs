using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionDelay = 3f; // Delay/Bomb Animation (DE)
    public float explosionRadius = 5f; // Bomb Radius
    public float explosionForce = 1000f; // Bomb Strength
    public float upwardsModifier = 0.5f; // Gravity/Velocity of Push

    public LayerMask affectedLayers;

    public GameObject explosionEffect;
    public AudioClip explosionSound; // Bomb sound

    private bool exploded = false;

    void Start()
    {
        Invoke("Explode", explosionDelay);
    }

    // Bomb/Explode Function
    private void Explode()
    {
        if (exploded)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            if (((1 << nearbyObject.gameObject.layer) & affectedLayers) != 0)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Push/Explode Objects away
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
                }
            }
        }

        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Play explosion sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        exploded = true;

        // Destroy the bomb object after explosion
        Destroy(gameObject);
    }
}

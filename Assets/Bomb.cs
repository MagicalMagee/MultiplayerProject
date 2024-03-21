using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionDelay = 3f;
    public float explosionRadius = 5f;
    public float explosionForce = 1000f;
    public float upwardsModifier = 0.5f;

    public LayerMask affectedLayers;

    public GameObject explosionEffect;
    public AudioClip explosionSound;

    private bool exploded = false;

    void Start()
    {
        Invoke("Explode", explosionDelay);
    }

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

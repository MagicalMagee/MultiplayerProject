using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 2;
    public float damageDelay = 2.0f;

    private bool canDealDamage = true;

    private void OnCollisionStay(Collision coll)
    {
        if (canDealDamage && coll.gameObject.tag == "Player")
        {
            Health playerHealth = coll.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                StartCoroutine(DealDamageWithDelay(playerHealth));
            }
        }
    }

    private IEnumerator DealDamageWithDelay(Health playerHealth)
    {
        canDealDamage = false;
        playerHealth.TakeDamage(damage);
        yield return new WaitForSeconds(damageDelay);
        canDealDamage = true;
    }
}

using UnityEngine;

public class ZombieCharacterControl : MonoBehaviour
{
    [SerializeField] private float m_initialMoveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_speedIncreaseRate = 0.1f;

    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;

    private Transform[] m_playerTransforms; // Store references to all players
    private float m_moveSpeed;

    private readonly float m_interpolation = 10;
    private float m_elapsedTime = 0f;

    private void Awake()
    {
        if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }

        m_moveSpeed = m_initialMoveSpeed;

        // Start the coroutine to periodically check for players
        StartCoroutine(CheckForPlayersCoroutine());
    }

    private System.Collections.IEnumerator CheckForPlayersCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Wait for 5 seconds before checking for players again
            FindPlayers();
        }
    }

    private void FindPlayers()
    {
        // Find all player objects by tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        m_playerTransforms = new Transform[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            m_playerTransforms[i] = players[i].transform;
        }
    }

    private void FixedUpdate()
    {
        if (m_playerTransforms != null && m_playerTransforms.Length > 0)
        {
            ChaseClosestPlayer();
            IncreaseSpeedOverTime();
        }
    }

    private void ChaseClosestPlayer()
    {
        // Find the closest player
        Transform closestPlayer = m_playerTransforms[0];
        float closestDistance = Vector3.Distance(transform.position, closestPlayer.position);
        foreach (Transform player in m_playerTransforms)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        // Calculate direction towards the closest player
        Vector3 direction = (closestPlayer.position - transform.position).normalized;

        // Rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * m_turnSpeed);

        // Move towards the player
        transform.position += transform.forward * m_moveSpeed * Time.deltaTime;

        // Set animator parameter for movement
        m_animator.SetFloat("MoveSpeed", m_moveSpeed);
    }

    private void IncreaseSpeedOverTime()
    {
        // Increase the move speed over time
        m_elapsedTime += Time.deltaTime;
        m_moveSpeed = m_initialMoveSpeed + m_speedIncreaseRate * m_elapsedTime;
    }
}
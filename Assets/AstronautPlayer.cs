using UnityEngine;
using System.Collections;
using Unity.Netcode;

namespace AstronautPlayer
{
    public class AstronautPlayer : MonoBehaviour
    {
        private Animator anim;
        private CharacterController controller;
        private AudioSource audioSource;

        public float speed = 600.0f;
        public float sprintSpeed = 900.0f; // Speed when sprinting
        public float turnSpeed = 400.0f;
        private Vector3 moveDirection = Vector3.zero;
        public float gravity = 20.0f;

        // Sprint ability variables
        public float sprintDuration = 2.0f; // Duration of sprint in seconds
        public float sprintCooldown = 5.0f; // Cooldown
        private bool isSprinting = false;
        private float sprintEndTime = 0.0f;
        private float sprintCooldownEndTime = 0.0f;

        // Push ability variables
        public float pushForce = 10.0f;
        public float pushCooldown = 3.0f;
        private float lastPushTime = -9999.0f;
        public AudioClip pushSound;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            anim = gameObject.GetComponentInChildren<Animator>();
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        

        void Update()
        {
            if (Input.GetKey("w"))
            {
                anim.SetInteger("AnimationPar", 1);
            }
            else
            {
                anim.SetInteger("AnimationPar", 0);
            }

            if (controller.isGrounded)
            {
                // Check if sprint is active and set movement speed accordingly
                float currentSpeed = isSprinting ? sprintSpeed : speed;
                moveDirection = transform.forward * Input.GetAxis("Vertical") * currentSpeed;
            }

            float turn = Input.GetAxis("Horizontal");
            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
            controller.Move(moveDirection * Time.deltaTime);
            moveDirection.y -= gravity * Time.deltaTime;

            // Sprint logic
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > sprintCooldownEndTime)
            {
                isSprinting = true;
                sprintEndTime = Time.time + sprintDuration;
            }

            if (isSprinting && Time.time > sprintEndTime)
            {
                isSprinting = false;
                sprintCooldownEndTime = Time.time + sprintCooldown;
            }

            // Check for E key input to push objects away
            if (Input.GetKeyDown(KeyCode.E) && Time.time - lastPushTime > pushCooldown)
            {
                PushObjectsAway();
                lastPushTime = Time.time;

                if (pushSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(pushSound);
                }
            }
        }

        void PushObjectsAway()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5.0f); // Adjust the radius as needed
            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null && rb != controller.attachedRigidbody)
                {
                    Vector3 direction = (col.transform.position - transform.position).normalized;
                    rb.AddForce(direction * pushForce, ForceMode.Impulse);
                }
            }
        }

        // Collision detection with "Star" objects
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Star"))
            {
                // Increase player's health
                Health playerHealth = GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(10); // Assuming healing the player by 10 when colliding with a star
                    Debug.Log("Player collided with a star! Health increased by 10. Current health: " + playerHealth.health);
                }
            }
        }
    }
}

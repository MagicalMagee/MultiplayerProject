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
        public float sprintSpeed = 900.0f; // Speed
        public float turnSpeed = 400.0f;
        private Vector3 moveDirection = Vector3.zero;
        public float gravity = 20.0f;

        // Sprint ability variables
        public float sprintDuration = 2.0f; // Duration of sprint
        public float sprintCooldown = 5.0f; // Cooldown
        private bool isSprinting = false;
        private float sprintEndTime = 0.0f;
        private float sprintCooldownEndTime = 0.0f; // Cooldown

        // Push ability variables
        public float pushForce = 10.0f; // Strength of Push
        public float pushCooldown = 3.0f; // Cooldown
        private float lastPushTime = -9999.0f; // Check
        public AudioClip pushSound; // Push Sound

        void Start()
        {
            controller = GetComponent<CharacterController>();
            anim = gameObject.GetComponentInChildren<Animator>();
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        

        void Update()
        {
            // Forward + Walk Animation
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

                // Force Push Sound
                if (pushSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(pushSound);
                }
            }
        }

        // Force Push Ability
        void PushObjectsAway()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5.0f); // Radius for Force push-able objects
            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null && rb != controller.attachedRigidbody)
                {
                    Vector3 direction = (col.transform.position - transform.position).normalized;
                    // Actual force push
                    rb.AddForce(direction * pushForce, ForceMode.Impulse);
                }
            }
        }

        // Star Collision, Heals Players for 10 HP
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Star"))
            {
                // Increase player's health
                Health playerHealth = GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(10); // Heal by 10
                }
            }
        }
    }
}

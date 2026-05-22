using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    private Animator animator;
    private AttackScript attackscript;
    private CharacterController controller;

    [Header("Dodge Settings")]
    public float dodgeCooldown = 1f;
    private float nextDodgeTime;
    public float ManageStamina = 20f;

    [Header("Dash Movement")]
    public float dodgeSpeed = 10f;

    private Vector3 dodgeDirection;

    [Header("Double Tap Settings")]
    public float doubleTapTime = 0.3f;

    private float lastTapA;
    private float lastTapD;

    public bool isDodging;
    public EntityStats entityStats;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackscript = GetComponent<AttackScript>();
        controller = GetComponent<CharacterController>();
        entityStats = GetComponent<EntityStats>();
    }

    void Update()
    {
        HandleDodgeInput();

        // 🔥 DASH MOVEMENT
        if (isDodging)
        {
            controller.Move(dodgeDirection * dodgeSpeed * Time.deltaTime);
        }
    }

    void HandleDodgeInput()
    {
        if (isDodging) return;
        if (Time.time < nextDodgeTime) return;

        // 🔥 DOUBLE TAP A
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastTapA <= doubleTapTime)
            {
                TryDodge(Vector3.left);
            }

            lastTapA = Time.time;
        }

        // 🔥 DOUBLE TAP D
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastTapD <= doubleTapTime)
            {
                TryDodge(Vector3.right);
            }

            lastTapD = Time.time;
        }
    }

    public void TryDodge(Vector3 direction)
    {
        if (isDodging) return;
        if (Time.time < nextDodgeTime) return;

        isDodging = true;

        // 🔥 SAVE DASH DIRECTION
        entityStats.ConsumeStamina(ManageStamina);
        dodgeDirection = direction.normalized;

        // ⚔️ CANCEL ATTACK
        if (attackscript != null)
        {
            attackscript.CancelAttack();
        }

        // 🔥 UPDATE ANIMATOR DIRECTION
        animator.SetFloat("x", dodgeDirection.x);
        animator.SetFloat("y", dodgeDirection.z);

        animator.SetBool("isDodging", true);
        animator.SetTrigger("Dodge");

        nextDodgeTime = Time.time + dodgeCooldown;
    }

    // 🎬 ANIMATION EVENT
    public void EndDodge()
    {
        isDodging = false;

        animator.SetBool("isDodging", false);

        dodgeDirection = Vector3.zero;
    }
}
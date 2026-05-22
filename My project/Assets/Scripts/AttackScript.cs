using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    private Animator animator;
    private PlayerDodge dodgescript;

    [Header("Combat")]
    public bool isAttacking = false;

    [Header("Timing")]
    public float attackCooldown = 0.35f;
    public float comboResetTime = 0.9f;

    private float lastClickTime;
    private float nextAttackTime;

    private int comboStep = 0;
    private bool inputBuffered = false;

    [Header("Attack Collsion")]
    public CapsuleCollider attackPoint;
    EntityStats entityStats;

    void Start()
    {
        animator = GetComponent<Animator>();
        dodgescript = GetComponent<PlayerDodge>();
        entityStats = GetComponent<EntityStats>();
    }

    void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
        inputBuffered = true;
    }

    if (!dodgescript.isDodging)
    {
        if (inputBuffered && Time.time >= nextAttackTime)
        {
            inputBuffered = false;
            DoAttack();
        }
    }

    if (Time.time - lastClickTime > comboResetTime)
    {
        comboStep = 0;
        animator.SetInteger("Combo", 0);
    }
}

    void DoAttack()
    {
        lastClickTime = Time.time;

        attackPoint.enabled = true;
        isAttacking = true;

        comboStep++;

        if (comboStep > 3)
            comboStep = 1;

        nextAttackTime = Time.time + attackCooldown;

        animator.SetInteger("Combo", comboStep);
        animator.SetTrigger("Attack");
    }

    public void EndAttack()
    {
        isAttacking = false;
        attackPoint.enabled = false;
    }

    public void CancelAttack()
    {
        attackPoint.enabled = false;
        isAttacking = false;
        inputBuffered = false;

        comboStep = 0;

        animator.ResetTrigger("Attack");
        animator.SetInteger("Combo", 0);
    }
}
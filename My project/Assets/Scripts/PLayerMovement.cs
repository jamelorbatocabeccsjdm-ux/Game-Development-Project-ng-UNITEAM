using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Animator animator;

    private CharacterController controller;
    private AttackScript attackscript;
    private PlayerDodge dodgescript;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Animation")]
    private float _lastX;
    private float _lastZ;

    [Header("Physics")]
    public float gravity = -9.8f;
    public float groundedForce = -2f;

    private Vector3 _velocity;

    public Transform virtualCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        attackscript = GetComponent<AttackScript>();
        dodgescript = GetComponent<PlayerDodge>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyGravity();
    }

    void ApplyMovement()
    {
        // 🔥 1. HIGHEST PRIORITY: DODGE (never interrupt movement logic incorrectly)
        if (dodgescript != null && dodgescript.isDodging)
        {
            animator.SetFloat("isMoving", 0);
            return;
        }

        // ⚔️ 2. ATTACK BLOCKS MOVEMENT (but NOT dodge)
        if (attackscript != null && attackscript.isAttacking)
        {
            animator.SetFloat("isMoving", 0);
            return;
        }

        // 🏃 3. NORMAL MOVEMENT INPUT
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = orientation.forward * z + orientation.right * x;
        moveDir.y = 0;

        float moveAmount = moveDir.magnitude;

        animator.SetFloat("isMoving", moveAmount);

        // Save last direction for idle blend
        if (x != 0 || z != 0)
        {
            _lastX = x;
            _lastZ = z;
        }

        animator.SetFloat("x", _lastX);
        animator.SetFloat("y", _lastZ);

        controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = groundedForce;
        }

        _velocity.y += gravity * Time.deltaTime;

        controller.Move(_velocity * Time.deltaTime);
    }

    public void CamShake(float frequency, float intensity)
    {
        if (virtualCamera != null)
        {
            CinemachineVirtualCamera vcam = virtualCamera.GetComponent<CinemachineVirtualCamera>();
            var noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise != null)
            {
                noise.m_AmplitudeGain = intensity;
                noise.m_FrequencyGain = frequency;
            }
        }
    }
}
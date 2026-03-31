using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    CharacterController controller;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;

    public float jumpStrength = 1.5f;

    [Header("Physics")]
    public float gravity = -9.8f; 
    public float groundedForce = -2f;
    private Vector3 _velocity;

    #region Built in Methods
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ApplyJump();
        }
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyGravity();
    }

    void LateUpdate()
    {
        
    }
    #endregion

    #region Movement Methods

    void ApplyMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = orientation.forward * z + orientation.right * x;
        moveDir.y = 0;

        float MoveVel = Mathf.Abs(moveDir.magnitude);
        
        animator.SetFloat("x", x);
        animator.SetFloat("y", z);
        animator.SetFloat("isMoving", MoveVel);

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

    public void ApplyJump()
    {
        if(!controller.isGrounded) return;
        _velocity.y = Mathf.Sqrt(groundedForce * gravity * jumpStrength);
    }

    #endregion

}



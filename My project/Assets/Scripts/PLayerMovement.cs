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

    [Header("Animation")]
    private float _lastX;
    private float _lastZ;

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
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
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
        
        animator.SetFloat("isMoving", MoveVel);

        if(x != 0 || z != 0)
        {
            _lastX = x;
            _lastZ = z;
        }

        SetFloat(_lastX, _lastZ);
        controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
    }

    void SetFloat(float x, float y)
    {
        animator.SetFloat("x", x);
        animator.SetFloat("y", y);
    }

#region Gravity and Jumping
    void ApplyGravity()
    {
        if (controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = groundedForce;
        }

        _velocity.y += gravity * Time.deltaTime;

        controller.Move(_velocity * Time.deltaTime);
    }
    #endregion
#endregion

    #region Attack
    void Attack()
    {
        animator.SetTrigger("Attack");
    }
    #endregion

}



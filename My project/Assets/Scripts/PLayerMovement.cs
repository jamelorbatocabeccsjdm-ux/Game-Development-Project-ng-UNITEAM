using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator _animator;
    private float horizontalInputt;
    private float verticalInput;
    Vector3 movement;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        horizontalInputt = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        movement = new Vector3(horizontalInputt, 0, verticalInput) * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
       
        if (horizontalInputt > 0)
        {
         gameObject.transform.localScale = new Vector3(5, 5, 5);
        }
        else if (horizontalInputt < 0)
        {
            gameObject.transform.localScale = new Vector3(-5, 5, 5);
        }
            

        // Set animation booleans
        bool isMovingHorizontal = Mathf.Abs(horizontalInputt) > 0.01f;
        _animator.SetBool("isRunning", isMovingHorizontal);

        if (verticalInput < 0 && !isMovingHorizontal)
        {
            _animator.SetBool("isRunningToward", true);
        }
        else
        {
            _animator.SetBool("isRunningToward", false);
        }
        
        if(verticalInput > 0 && !isMovingHorizontal)
        {
            _animator.SetBool("isBackward", true);
        }
        else
        {
            _animator.SetBool("isBackward", false);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float speed = 5f;
    private float horizontalInputt;
    private float verticalInput;
    Vector3 movement;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalInputt = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        movement = new Vector3(horizontalInputt, 0, verticalInput) * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }
}

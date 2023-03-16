using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;

    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;

        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        // when the tank is turned off, set it to kinematic so it stops moving 
        m_Rigidbody.isKinematic = true;
    }

    // Update is called once per frame 
    private void Update()
    {
        m_MovementInputValue = Input.GetAxis("Vertical");
        m_TurnInputValue = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        // create a vector in the direction the tank is facing with a magnitude  
        // based on the input, speed and time between frames 
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position 
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn()
    {
        // determine the number of degrees to be turned based on the input,  
        // speed and time between frames 
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // make this into a rotation in the y axis 
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // apply this rotation to the rigidbody's rotation 
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
} 

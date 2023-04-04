using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;

    public Transform m_target;

    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;


    float cameraSize;
    float currentSize;
    float maxSize = 18;
    float minSize = 5;

    private void Awake()
    {
        m_target = GameObject.FindGameObjectWithTag("Player").transform;
        cameraSize = Camera.main.orthographicSize;
        currentSize = cameraSize;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        m_DesiredPosition = m_target.position;
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log(scroll);
        if (scroll < 0)
        {
            if (currentSize <= maxSize)
            {
                currentSize -= scroll * 2;
                Camera.main.orthographicSize = currentSize;
            }
            
        } else if (scroll > 0)
        {
            if (currentSize >= minSize)
            {
                currentSize -= scroll * 2;
                Camera.main.orthographicSize = currentSize;
            }
            
        }
    }
}

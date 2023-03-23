using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTankMovement : MonoBehaviour
{
    // Take stops moving towards player at certain distance
    public float m_CloseDistance = 8f;
    // Tank's turret
    public Transform m_Turret;

    // A reference to the player
    private GameObject m_Player;
    // A reference to the enemy base
    private GameObject m_Base;
    // A reference to the navmesh agent component
    private NavMeshAgent m_NavAgent;
    // A reference to the rigidbody component
    private Rigidbody m_Rigidbody;

    private Transform m_TankRenderers;


    // Set to true when tank should follow player
    private bool m_Follow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Base = GameObject.FindGameObjectWithTag("EnemyBase");
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TankRenderers = gameObject.transform.Find("TankRenderers");
        m_Turret = m_TankRenderers.transform.Find("TankTurret");
        m_Follow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Follow == false)
        {
            // Get distance from enemy to enemy base
            float _distance = (m_Base.transform.position - transform.position).magnitude;
            // If distance is less than stop distance then stop moving
            if (_distance > m_CloseDistance)
            {
                m_NavAgent.SetDestination(m_Base.transform.position);
                m_NavAgent.isStopped = false;
            }
            else
            {
                m_NavAgent.isStopped = true;
            }

            if (m_Turret != null)
            {
               // Debug.Log("Test");
                m_Turret.LookAt(m_Base.transform);
            }


            return;
        }

        // Get distance from player to enemy
        float distance = (m_Player.transform.position - transform.position).magnitude;
        // If distance is less than stop distance then stop moving
        if (distance > m_CloseDistance)
        {
            m_NavAgent.SetDestination(m_Player.transform.position);
            m_NavAgent.isStopped = false;
        } else
        {
            m_NavAgent.isStopped = true;
        }

        if (m_Turret != null)
        {
            //Debug.Log("Test");
            m_Turret.LookAt(m_Player.transform);
        }


    }

    private void OnEnable()
    {
        // When tank is turned make sure its not kinematic
        m_Rigidbody.isKinematic = false;
    }

    private void OnDisable()
    {
        // When tank is turned off set kinematic to true so it stops moving
        m_Rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            m_Follow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            m_Follow = false;
        }
    }
}

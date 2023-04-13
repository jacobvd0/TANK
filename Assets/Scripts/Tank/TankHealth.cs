using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    // Amount of health each tank starts with
    public float m_StartingHealth = 100f;

    // Prefab that will  be insitaged in awake then used whnever the tank dies
    public GameObject m_ExplosionPrefab;
    public GameObject m_TankPrefab;

    private float m_CurrentHealth;
    private bool m_Dead;

    // Particle system that will be played when tank is destroyed
    private ParticleSystem m_ExplosionParticles;

    // Enemy base location
    private GameObject m_Base;

    private void Awake()
    {
        // Instantiate the explosion prefab and get a reference to the particle system on it
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

        // Disable the prefab so it can be activated when needed
        m_ExplosionParticles.gameObject.SetActive(false);

        // Find enemy base
        m_Base = GameObject.FindGameObjectWithTag("EnemyBase");
    }

    private void OnEnable()
    {
        // when tank is enabledm reset health and if its dead or not
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    private void SetHealthUI()
    {
        // Do later
    }

    public void TakeDamage(float amount)
    {
        // reduce health by amount of dmg done
        m_CurrentHealth -= amount;

        // Update UI
        SetHealthUI();

        // If health is at or below 0 call OnDeath
        if (m_CurrentHealth <= 0 && !m_Dead)
        {
            OnDeath();
        } 
    }

    private void OnDeath()
    {
        // set dead to true so functions dont repeat multiple times
        m_Dead = true;

        // Move the instantiated explosion prefab to the tanks position and enable it
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        
        

        // Play explosion particles
        m_ExplosionParticles.Play();

        // Turn tank off
        gameObject.SetActive(false);

        //if (m_TankPrefab.gameObject != null)
        //{
        //    var newTank = Instantiate(m_TankPrefab, m_Base.transform.position, m_Base.transform.rotation);
        //    newTank.gameObject.transform.position = m_Base.transform.position;
        //    newTank.gameObject.SetActive(true);


        //}
        //  ^ disabled this since tanks spawn back after rounds instead now (leaving this on makes extra spawn)

    }



}

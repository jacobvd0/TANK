using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    // Time before shell is removed (seconds)
    public float m_MaxLifeTime = 2f;
    // Amount of damage done if centered on a tank
    public float m_MaxDamage = 34f;
    // Explosion radius
    public float m_ExplosionRadius = 5;
    // The amount of force added to a tank at the center of the explosion
    public float m_ExposionForce = 100f;

    // Reference to the particels that will play on explosion
    public ParticleSystem m_ExplosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        // find the rigidbody of the collision object
        Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

        // Add code to damage tank here

        // Unparent particles from shell
        m_ExplosionParticles.transform.parent = null;

        // Play the particle effect
        m_ExplosionParticles.Play();

        // Once particles finished, destroy gameobject they're on
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);

        // Destroy shell
        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

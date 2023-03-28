using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
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

        // Only tanks will have rigidbody scripts
        if (targetRigidbody != null)
        {
            // Add an explosion force
            targetRigidbody.AddExplosionForce(m_ExposionForce, transform.position, m_ExplosionRadius);

            // Find the tankhealth script
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (targetHealth != null)
            {
                // Calc amount of damage the target should take depending on distance from shell
                float damage = CalculateDamage(targetRigidbody.position);

                // Deal health to tank
                targetHealth.TakeDamage(damage);
            }
        }

        // Unparent particles from shell
        m_ExplosionParticles.transform.parent = null;

        // Play the particle effect
        m_ExplosionParticles.Play();

        // Once particles finished, destroy gameobject they're on
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);

        // Destroy shell
        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        // Create a vector from the shell to the target
        Vector3 explosionToTarget = targetPosition - transform.position;
        // Calculate the distance from the shell to the target
        float explosionDistance = explosionToTarget.magnitude;
        // Calculate the proportion of the maximum distance (the explosionRadius)
        // the target is away
        float relativeDistance =
        (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
        // Calculate damage as this proportion of the maximum possible damage
        float damage = relativeDistance * m_MaxDamage;
        // Make sure that the minimum damage is always 0
        damage = Mathf.Max(0f, damage);
        return damage;
    }
}

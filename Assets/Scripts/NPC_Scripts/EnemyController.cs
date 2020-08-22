using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public float interactRadius = 5f;

    public float interactRate = 0.01f;
    public float timeBeforeInteract = 0.8f;
    protected float nextTimeToInteract = 0f;

    protected Transform target;
    protected NavMeshAgent agent;

    protected Animator anim;

    public float health = 50f;

    protected bool deathTransition = false;
    protected bool flag = true;

    public int essenceDrop;

    /// <summary>
    /// Unity specific method
    /// </summary>
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        agent.autoBraking = true;

        if(anim == null)
        {
            throw new System.ArgumentException("The following gameobject has no animator. Make sure to have an animator component");
        }

        flag = true;
    }

    /// <summary>
    /// Unity specific method
    /// </summary>
    void Update()
    {
        interaction();
    }

    /// <summary>
    /// Interacts with the player or environment. Can be overriden in extended classes
    /// </summary>
    protected virtual void interaction()
    {
        faceTarget();
    }

    /// <summary>
    /// A simple method to face target
    /// </summary>
    protected void faceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    /// <summary>
    /// When enemy controller dies.
    /// </summary>
    protected virtual void death()
    {
        Destroy(gameObject, 1f);
    }

    /// <summary>
    /// Events when enemy takes damage.
    /// </summary>
    /// <param name="amount"></param>
    public virtual void takeDamage(float amount)
    {
        health -= amount;
    }
    
    /// <summary>
    /// Draws a red sphere around gameobject to determine within interaction range
    /// </summary>
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    
}

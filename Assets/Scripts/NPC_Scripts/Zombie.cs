using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : EnemyController
{
    void Start()
    {
        base.Start();
    }

    protected override void interaction()
    {
        float distance = (transform.position - target.position).magnitude;

        if (distance <= lookRadius)
        {
            faceTarget();

            if (distance > interactRadius)
            {
                agent.isStopped = false;
                anim.SetTrigger("ToWalk");

                agent.SetDestination(target.position);

                this.nextTimeToInteract = Time.time + this.timeBeforeInteract;
            }
            else
            {
                agent.isStopped = true;
                anim.SetTrigger("ToAttack");

                attack();
            }
        }
        else
        {
            agent.isStopped = true;
            anim.SetTrigger("ToIdle");
        }
    }

    protected override void death()
    {
        deathTransition = true;

        anim.SetTrigger("OnDeath");
        GameState.changeZombieCount(-1);
        GameState.changeKillCount(1);
        GameState.addEssence(essenceDrop);
        Destroy(gameObject, 1f);
    }

    public override void takeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f && flag)
        {
            flag = false;

            this.death();
        }
    }

    protected void attack()
    {
        if (Time.time > nextTimeToInteract && !deathTransition)
        {
            nextTimeToInteract = Time.time + 1f / interactRate;

            PlayerManager.instance.player.GetComponent<PlayerStats>().decreaseHealth(10);
        }

    }

    IEnumerator delayInteract()
    {
        yield return new WaitForSeconds(0.1f);
        attack();
    }

}

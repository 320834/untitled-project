using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Node : Spawner
{
    public float maxHealth;
    public float health;

    public int essenceDrop;

    public float healRate = 0.1f;
    private float nextTimeToHeal = 0f;

    public bool takingDamage;
    public float timeDamage;

    public float groupSpawn = 5f;
    public float spawnRange = 4f;

    public GameObject zombie;

    public Slider healthBar;

    public Text destroyMessage;

    // Start is called before the first frame update
    void Start()
    {
        //Add to spawn state
        SpawnState.addSpawner(this);

        health = 2000;
        GameState.addNode();
        this.takingDamage = false;

        timeDamage = 3f;
        groupSpawn = 5;

        healthBar.minValue = 0;
        healthBar.maxValue = 100;

        
    }

    // Update is called once per frame
    void Update()
    {
        interaction();

        healthBar.value = (this.health/this.maxHealth) * 100;
    }

    /// <summary>
    /// Override from EnemyController.
    /// When health 
    /// </summary>
    protected void interaction()
    {
        if (this.health <= 0)
        {
            this.death();
        }

        //if(Time.time > nextTimeToSpawn)
        //{
        //    this.nextTimeToSpawn = Time.time + (1 / this.spawnRate);
        //    spawnGroup();
        //}

        this.takingDamage = false;
        heal();
    }

    /// <summary>
    /// Call the gamestate to destory a node when it runs out of health
    /// </summary>
    private void destory()
    {
        GameState.addEssence(essenceDrop);
        GameState.destoryNode();
        SpawnState.removeSpawner(this);
    }
    
    /// <summary>
    /// Heals the node if the node has taken taken damage.
    /// </summary>
    private void heal()
    {
        if (Time.time > nextTimeToHeal && Time.time > timeDamage && health <= maxHealth)
        {
            nextTimeToHeal = Time.time + (1f / healRate);

            health += 0.5f;
        }
    }

    /// <summary>
    /// Actions on death.
    /// </summary>
    protected void death()
    {
        //string mess = "";

        //if(GameState.nodesAlive != 0)
        //{
        //    mess = "There are " + GameState.nodesAlive + " nodes left";
        //}
        //else
        //{
        //    mess = "All nodes destroyed. Portal shield down";
        //}

        //StartCoroutine(showMessage(mess, 3f, destroyMessage));
        this.destory();
        Destroy(gameObject);
    }
   
    /// <summary>
    /// When player deals damage to the node.
    /// </summary>
    /// <param name="amount">Amount of damage to deal</param>
    public override void takeDamage(float amount)
    {
        //The time it takes before node heals after taking damage
        timeDamage = Time.time + 5f;

        health -= amount;
    }

    /// <summary>
    /// Spawns one zombie
    /// </summary>
    protected void spawn(float health, int drop, int speed)
    {
        float x = transform.position.x;
        float z = transform.position.z;

        Vector3 newPos = new Vector3(Random.Range(x - spawnRange, x + spawnRange), transform.position.y, Random.Range(z - spawnRange, z + spawnRange));
        GameObject octo = Instantiate(zombie, newPos, Quaternion.identity) as GameObject;

        //Set speed, health, and drop
        octo.GetComponent<Zombie>().health = health;
        octo.GetComponent<Zombie>().essenceDrop = drop;
        octo.GetComponent<NavMeshAgent>().speed = speed;

        octo.SetActive(true);
        GameState.changeZombieCount(1);
    }

    /// <summary>
    /// Spawns a group of zombie with a random spread of zombies.
    /// </summary>
    public override void spawnGroup(float health, int drop, int speed)
    {
        float spread = 2f;
        float lowerBound = groupSpawn + spread;
        float upperBound = groupSpawn - spread;
        int amount = (int)Random.Range(lowerBound, upperBound);
        for(int i = 0; i < amount; i++)
        {
            spawn(health, drop, speed);
        }
    }

    //IEnumerator showMessage(string mess, float delay, Text panel)
    //{
    //    panel.text = mess;

    //    yield return new WaitForSeconds(delay);

    //    panel.text = "";
    //}
}

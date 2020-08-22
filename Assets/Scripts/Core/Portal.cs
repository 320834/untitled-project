using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Portal : Spawner
{
    public float maxHealth;
    public float health;

    public float healRate;
    private float nextTimeToHeal = 0f;

    public bool takingDamage;
    public float timeDamage;

    public float groupSpawn = 5f;
    public float spawnRange = 4f;

    public GameObject zombie;

    public Slider healthBar;
    public Image shieldFill;

    private bool shieldUp;

    void Start()
    {
        //Add to spawn state
        SpawnState.addSpawner(this);

        takingDamage = false;

        timeDamage = 3f;
        groupSpawn = 5;

        healthBar.minValue = 0;
        healthBar.maxValue = 100;

        shieldUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        interaction();
    }

    protected override void interaction()
    {
        if (this.health <= 0)
        {
            this.death();
            this.destroy();
        }

        healthBar.value = (health / maxHealth) * 100;

        this.takingDamage = false;
        heal();

        if (GameState.nodesAlive == 0)
        {
            shieldFill.gameObject.SetActive(false);
            shieldUp = false;
        }
    }

    private void destroy()
    {
        GameState.destoryPortal();



        SpawnState.portal = null;
        SpawnState.removeSpawner(this);
    }

    private void heal()
    {
        if (Time.time > nextTimeToHeal && Time.time > timeDamage && health <= maxHealth)
        {
            nextTimeToHeal = Time.time + (1f / healRate);

            health += 0.5f;
        }
    }

    protected override void death()
    {
        Destroy(gameObject);
    }

    public override void takeDamage(float amount)
    {
        if(!shieldUp)
        {
            timeDamage = Time.time + 5f;

            health -= amount;
        }
    }

    protected void spawn(float health, int drop, int speed)
    {
        float x = transform.position.x;
        float z = transform.position.z;

        Vector3 newPos = new Vector3(Random.Range(x - spawnRange, x + spawnRange), transform.position.y, Random.Range(z - spawnRange, z + spawnRange));
        GameObject octo = Instantiate(zombie, newPos, Quaternion.identity) as GameObject;

        //Set speed, health and drop
        octo.GetComponent<Zombie>().health = health;
        octo.GetComponent<Zombie>().essenceDrop = drop;
        octo.GetComponent<NavMeshAgent>().speed = speed;

        octo.SetActive(true);
        GameState.changeZombieCount(1);
    }

    public override void spawnGroup(float health, int drop, int speed)
    {
        float spread = 2f;
        float lowerBound = groupSpawn + spread;
        float upperBound = groupSpawn - spread;
        int amount = (int)Random.Range(lowerBound, upperBound);
        for (int i = 0; i < amount; i++)
        {
            spawn(health, drop, speed);
        }
    }
}

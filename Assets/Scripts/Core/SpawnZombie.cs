using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnZombie : Spawner
{
    public GameObject enemy;
    public float range;
    public int initialSpawn = 5;

    private double nextTimeToSpawn = 0f;
    public double spawnRate = 2f;

    // Start is called before the first frame update
    void Start()
    {
        initialSpawn = 5;
        SpawnState.addSpawner(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawn(float health, int drop, int speed)
    {
        float x = transform.position.x;
        float z = transform.position.z;

        Vector3 newPos = new Vector3(Random.Range(x - range, x + range), transform.position.y, Random.Range(z - range, z + range));
        GameObject octo = Instantiate(enemy, newPos, Quaternion.identity) as GameObject;

        //Set appriopriate settings
        octo.GetComponent<Zombie>().health = health;
        octo.GetComponent<Zombie>().essenceDrop = drop;
        octo.GetComponent<NavMeshAgent>().speed = speed;

        octo.SetActive(true);
        GameState.changeZombieCount(1);
    }

    public override void spawnGroup(float health, int drop, int speed)
    {
        for(int i = 0; i < initialSpawn; i++)
        {
            spawn(health, drop, speed);
        }
       
    }
}

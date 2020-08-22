using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using System.Linq;


public class SpawnState : MonoBehaviour
{
    public static List<Spawner> spawnerList = new List<Spawner>();
    public static Portal portal;

    private static double nextTimeToSpawn = 0f;
    public static double spawnRate = 0.5f;
    public static int maxEnemies = 30;

    public static int activeSpawners;

    private static int currentSpawnIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        //Spawn from portal first
        currentSpawnIndex = 0;
        activeSpawners = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    /// <summary>
    /// Add node to spawn list
    /// </summary>
    /// <param name="newNode">A new node</param>
    public static void addSpawner(Spawner newSpawner)
    {
        spawnerList.Add(newSpawner);
    }

    /// <summary>
    /// Remove node from spawn list
    /// </summary>
    /// <param name="node">The node to remove</param>
    public static void removeSpawner(Spawner current)
    {
        spawnerList.Remove(current);
    }

    /// <summary>
    /// Spawns enemy from an index 
    /// </summary>
    /// <param name="spawnIndex"></param>
    /// <returns>True if found and active, false if not found or active</returns>
    public static bool spawnIndex(int spawnIndex)
    {
        if(spawnIndex >= 0 && spawnIndex < spawnerList.Count && spawnerList.ElementAt(currentSpawnIndex).active)
        {
            spawnerList.ElementAt(spawnIndex).spawnGroup(100f, 10, 2);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Spawn zombie group in a random active spawner
    /// </summary>
    /// <returns>True when it can spawn. False if it can't due to zombie limit</returns>
    public static bool spawnRandom(float health, int drop, int speed)
    {
        int index = Random.Range(0, spawnerList.Count);
        //UnityEngine.Debug.Log("Index: " + index);
        //UnityEngine.Debug.Log("List Count : " + spawnerList.Count);

        if(GameState.numOfEnemies >= maxEnemies)
        {
            return false;
        }

        while (!spawnerList.ElementAt(index).active)
        {
            // UnityEngine.Debug.Log(index);
            index = Random.Range(0, spawnerList.Count);
        }

        //UnityEngine.Debug.Log("Spawn at index " + spawnerList.ElementAt(index).spawnId);
        spawnerList.ElementAt(index).spawnGroup(health, drop, speed);
        return true;
    }

    /// <summary>
    /// Sequentially Spawn Zombies
    /// </summary>
    public static void sequentialSpawn()
    {
        if(currentSpawnIndex >= spawnerList.Count)
        {
            currentSpawnIndex = 0;
        }

        if(spawnerList.ElementAt(currentSpawnIndex).active)
        {
            if (spawnerList.Count != 0)
            {
                spawnerList.ElementAt(currentSpawnIndex).spawnGroup(100f, 10, 2);
            }
        }
        else
        {
            nextTimeToSpawn = Time.time;
        }

        currentSpawnIndex += 1;
    }

    public static void removeAllSpawners()
    {
        GameState.nodesAlive = 0;
        spawnerList.Clear();
    }
    
}

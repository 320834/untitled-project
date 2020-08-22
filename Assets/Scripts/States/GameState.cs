using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static int numOfEnemies = 0;
    public static int zombiesKilled = 0;

    public static int essence;

    public static int nodesAlive = 0;

    // Start is called before the first frame update
    void Start()
    {
        numOfEnemies = 0;
        zombiesKilled = 0;

        essence = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    /// <summary>
    /// Increases or decreases the zombie count
    /// </summary>
    /// <param name="count"></param>
    public static void changeZombieCount(int count)
    {
        numOfEnemies += count;
    }

    /// <summary>
    /// Increase or decrease the kill count 
    /// </summary>
    /// <param name="count"></param>
    public static void changeKillCount(int count)
    {
        zombiesKilled += count;
    }

    /// <summary>
    /// Increase the essence. Called from killing enemies, destroying nodes, or other events
    /// </summary>
    /// <param name="amount"></param>
    public static void addEssence(int amount)
    {
        essence += amount;
    }

    /// <summary>
    /// Determine essence is enough, if true deduct essence from field
    /// </summary>
    /// <param name="amount">The amount of essence to deduct</param>
    /// <returns>True if purchased, false if can't</returns>
    public static bool purchase(int amount)
    {
        if(essence >= amount)
        {
            essence = essence - amount;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Decrease the count of nodes in the game. Called in Node.cs
    /// </summary>
    public static void destoryNode()
    {
        nodesAlive--;
    }

    /// <summary>
    /// Destroy portal. Called in Portal.cs
    /// </summary>
    public static void destoryPortal()
    {
        //TODO: Go to win game
        // UnityEngine.Debug.Log("WIN GAME");
        PlayerManager.instance.player.GetComponent<PlayerInteract>().winGame = true;

        GameObject a = GameObject.Find("GameState");
        a.GetComponent<PlayerState>().endMenu();
        
    }

    /// <summary>
    /// Add a node to gamestate. Called only when Node is instantiated.
    /// </summary>
    public static void addNode()
    {
        nodesAlive += 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundState : MonoBehaviour
{
    private double nextTimeToSpawn = 0f;
    public float secondsBeforeNextSpawn;
    public int delayBetweenRounds;
    public int maxRounds;

    //Enemy stats
    public float healthBase;
    public int dropBase;
    public int speedBase;

    private float healthIncrease;
    private int dropIncrease;
    private int speedIncrease;

    public int[] listRound;

    public Text roundText;
    public GameObject beginningPanel;
    public bool start;
    

    private IEnumerator coroutine;

    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        int increment = 2;
        listRound = new int[maxRounds];

        for(int i = 0; i < maxRounds; i++)
        {
            listRound[i] = increment * (i + 1);
        }

        flag = true;
        start = false;


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;


    }

    // Update is called once per frame
    void Update()
    {

        if(start && flag && SpawnState.activeSpawners > 0)
        {
            coroutine = roundController();

            StartCoroutine(coroutine);

            flag = false;
        }
    }

    public IEnumerator round(int groups)
    {
        UnityEngine.Debug.Log("Round");
        for (int i = 0; i < groups;)
        {
            SpawnState.spawnRandom(100, 50, 2);

            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator roundController()
    {
        //IEnumerator localCoro;
        int roundNum = 1;
        foreach (int num in listRound)
        {
            //UnityEngine.Debug.Log("========================== NEW ROUND ================================");
            //UnityEngine.Debug.Log("Round Number: " + roundNum);
            //UnityEngine.Debug.Log("Groups: " + num);
            //UnityEngine.Debug.Log("=====================================================================");
            yield return new WaitForSeconds(delayBetweenRounds);

            StartCoroutine(showMessage("Round " + roundNum, 3f));

            for (int i = 0; i < num; i++)
            {

                float health = spawnHealth(i);
                int drop = spawnDrop(i);
                int speed = spawnSpeed(i);

                bool overlimit = SpawnState.spawnRandom(health, drop, speed);
                while(!overlimit)
                {
                    yield return new WaitForSeconds(0.5f);
                    overlimit = SpawnState.spawnRandom(health, drop, speed);
                }

                yield return new WaitForSeconds(secondsBeforeNextSpawn);
            }

            while (GameState.numOfEnemies > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            roundNum += 1;
        }
    }

    IEnumerator showMessage(string message, float duration)
    {
        roundText.text = message;

        yield return new WaitForSeconds(duration);

        roundText.text = "";
    }

    public void startGame()
    {
        beginningPanel.SetActive(false);
        start = true;

        PlayerManager.instance.player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        PlayerManager.instance.player.gameObject.GetComponent<MouseLook>().enabled = true;
        PlayerManager.instance.player.gameObject.GetComponentInChildren<GunScript>().disabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private float spawnHealth(int round)
    {
        return healthBase + (int)((round + 1) / 5) * 25;
    }

    private int spawnDrop(int round)
    {
        return dropBase + ((round + 1) / 5) * 10;
    }

    private int spawnSpeed(int round)
    {
        if(round >= 30)
        {
            return 5;
        }
        if(round >= 20)
        {
            return 4;
        }

        if(round >= 10)
        {
            return 3;
        }

        return speedBase;
    }
}

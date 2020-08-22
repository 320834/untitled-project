using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{

    public Camera firstPerson;
    public Camera overview;

    public GameObject player;

    public GameObject enemy;

    private bool toggle = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    if(toggle)
        //    {
        //        firstPerson.enabled = false;
        //        overview.enabled = true;

        //        toggle = false;
        //    }
        //    else
        //    {
        //        firstPerson.enabled = true;
        //        overview.enabled = false;

        //        toggle = true;
        //    }
            
        //}

        if(Input.GetKeyDown(KeyCode.E))
        {
            // PlayerManager.instance.player.GetComponent<PlayerHealth>().decreaseHealth(10);
            // player.GetComponent<PlayerHealth>().decreaseHealth(10);

            // Quaternion b = new Quaternion();
            // Vector3 a = new Vector3(7, -75, -21);

             // Instantiate(enemy, a);
        }


    }

    
}

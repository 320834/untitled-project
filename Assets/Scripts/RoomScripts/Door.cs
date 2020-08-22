using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Door : MonoBehaviour
{
    public string id;
    public bool interactable;

    //If interactable, true open, false close 
    public bool state;
    public int cost;

    public int direction;

    public List<GenericRoom> rooms = new List<GenericRoom>();


    // Start is called before the first frame update
    void Start()
    {
        this.state = false;
        //this.cost = 100;
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    /// <summary>
    /// To open the door
    /// </summary>
    public bool buy()
    {
        if(!state && GameState.purchase(cost))
        {
            this.setRoomsActive();
            this.open();

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Render open the door
    /// </summary>
    public void open()
    {
        //TODO later: For now, just make door disappear
        //this.gameObject.SetActive(false);NavMeshObstacle

        if(direction == 0)
        {
            Animation anim = gameObject.GetComponent<Animation>();
            gameObject.GetComponent<NavMeshObstacle>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;

            anim.Play("DoorOpen");
        }
        else
        {
            Animation anim = gameObject.GetComponent<Animation>();
            gameObject.GetComponent<NavMeshObstacle>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;

            anim.Play("DoorOpenX");
        }
        
    }

    /// <summary>
    /// Render close the door
    /// </summary>
    public void close()
    {
        //TODO later: For now, just make door disappear
        this.gameObject.SetActive(true);
    }

    private void setRoomsActive()
    {
        foreach(GenericRoom room in rooms)
        {
            room.setRoomActive();
        }
    }

}

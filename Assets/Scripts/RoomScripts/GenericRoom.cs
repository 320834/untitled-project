using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenericRoom : MonoBehaviour
{
    public string roomId;
    public Door north;
    public Door east;
    public Door south;
    public Door west;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        north = null;
        east = null;
        south = null;
        west = null;
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        // UnityEngine.Debug.Log(roomId + active);
        if (active)
        {
            Spawner spawner = GetComponentInChildren(typeof(Spawner)) as Spawner;

            if(spawner != null)
            {
                spawner.setStateActive();
            }
            
        }
    }

    /// <summary>
    /// Set the room active when door is opened
    /// </summary>
    public void setRoomActive()
    {
        active = true;
    }

    public void buildDoorNavMesh()
    {
        if(this.north != null)
        {
            this.north.GetComponentInParent<NavMeshSurface>().BuildNavMesh();
        }

        if(this.south != null)
        {
            this.south.GetComponentInParent<NavMeshSurface>().BuildNavMesh();
        }

        if (this.east != null)
        {
            this.east.GetComponentInParent<NavMeshSurface>().BuildNavMesh();
        }

        if (this.north != null)
        {
            this.north.GetComponentInParent<NavMeshSurface>().BuildNavMesh();
        }
    }
}

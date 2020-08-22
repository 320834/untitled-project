using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool active;
    public string spawnId;
    // Start is called before the first frame update
    void Start()
    {
        this.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setStateActive()
    {
        this.active = true;
        SpawnState.activeSpawners += 1;
    }

    public virtual void takeDamage(float amount) { }

    public virtual void spawnGroup(float health, int drop, int speed) { }

    protected virtual void interaction() { }

    protected virtual void death() { }
}

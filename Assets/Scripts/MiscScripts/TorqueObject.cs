using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueObject : MonoBehaviour
{
    public int torqueLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().AddTorque(transform.up * torqueLevel);
        gameObject.GetComponent<Rigidbody>().AddTorque(transform.right * 20);
    }
}

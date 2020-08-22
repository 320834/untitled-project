using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float force = 40f;
    public float fireRate = 0.2f;

    public Camera pov;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public bool isRecoiling;
    public float recoilAmount;

    public MouseLook mouselookScript;

    public bool own;

    //Ammo fields
    public int clipAmt;
    public int clipMax;

    public int totalAmmo;
    public Animation reloadAnimation;

    public Text ammoUI;

    public bool disabled;

    void Start()
    {
        disabled = false;
        own = true;
    }

    // Update is called once per frame
    void Update()
    {
        ammoUI.text = clipAmt.ToString() + " / " + totalAmmo.ToString();

        if(!disabled)
        {
            if (Input.GetButton("Fire1") && clipAmt > 0 && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();

                recoiling();

                clipAmt -= 1;
            }

            if (Input.GetButtonDown("Fire1") && clipAmt > 0)
            {
                muzzleFlash.Play();

                isRecoiling = true;
            }

            if (Input.GetButtonUp("Fire1"))
            {
                muzzleFlash.Stop();
                decoiling();
                isRecoiling = false;
            }

            if (Input.GetKeyDown(KeyCode.R) && clipAmt < clipMax && totalAmmo > 0)
            {
                reloadAnimation.Play();
                StartCoroutine(reload());
            }

            if (clipAmt == 0)
            {
                muzzleFlash.Stop();
                decoiling();
                isRecoiling = false;
            }
        }
    }

    /// <summary>
    /// Shoot 
    /// </summary>
    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(pov.transform.position, pov.transform.forward, out hit, range))
        {
            EnemyController targetNPC = getComponentNPC(hit);
            Spawner targetSpawner = getComponentSpawn(hit);

            if(targetNPC != null)
            {
                targetNPC.takeDamage(damage);
            }

            if(targetSpawner != null)
            {
                targetSpawner.takeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * force);
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 0.2f);
        }
    }

    void recoiling()
    {
        mouselookScript.recoilAmt = recoilAmount * Time.deltaTime;
    }

    void decoiling()
    {
        mouselookScript.recoilAmt = 0f;
    }

    /// <summary>
    /// Reload actions
    /// </summary>
    IEnumerator reload()
    {
        //Play animation 
        
        yield return new WaitForSeconds(1f);

        //Do calcuations
        int refill = clipMax - clipAmt;
        refill = totalAmmo > refill ? refill : totalAmmo;

        clipAmt = totalAmmo > clipMax ? clipMax : totalAmmo; 

        totalAmmo -= refill;

        

    }

    /// <summary>
    /// Return the correct component based on the raycasthit hit.
    /// </summary>
    /// <param name="hit">RaycastHit Object</param>
    /// <returns>EnemyController or null if none is found</returns>
    protected EnemyController getComponentNPC(RaycastHit hit)
    {
        if(hit.transform.GetComponent<Zombie>())
        {
            return hit.transform.GetComponent<Zombie>();
        }

        return null;
    }

    protected Spawner getComponentSpawn(RaycastHit hit)
    {
        if(hit.transform.GetComponent<Node>())
        {
            return hit.transform.GetComponent<Node>();
        }
        else if(hit.transform.GetComponent<Portal>())
        {
            return hit.transform.GetComponent<Portal>();
        }

        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public Camera pov;
    public float range;

    public GameObject[] weapons;
    public int currentWeapon;

    public GameObject shopPanel;
    public Text interactEvent;
    public Text notEnoughCredits;

    public GameObject startPanel;
    public GameObject pausePanel;

    private bool clearEventText;
    private bool outGame;

    public bool death;
    public bool winGame;


    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = 0;
        range = 5f;

        clearEventText = true;
        outGame = false;
        death = false;
        winGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(hover());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            outGame = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            getDoor();
            openShop();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            switchWeapons();
        }

        if(shopPanel.active || startPanel.active || outGame || death || winGame)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            gameObject.GetComponent<MouseLook>().enabled = false;
            gameObject.GetComponentInChildren<GunScript>().disabled = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.GetComponent<PlayerMovement>().enabled = true;
            gameObject.GetComponent<MouseLook>().enabled = true;
            gameObject.GetComponentInChildren<GunScript>().disabled = false;
        }
    }

    void getDoor()
    {
        RaycastHit hit;

        if(Physics.Raycast(pov.transform.position, pov.transform.forward, out hit, range))
        {
            
            Door door = hit.transform.GetComponent<Door>();

            if(door != null )
            {
                if(!door.buy())
                {
                    StartCoroutine(showMessage("Not Enough Credits", 2, notEnoughCredits));
                }
            }
        }
    }

    IEnumerator hover()
    {
        RaycastHit hit;
        clearEventText = true;
        if (Physics.Raycast(pov.transform.position, pov.transform.forward, out hit, range))
        {
            Door door = hit.transform.GetComponent<Door>();
            ShopRoom shop = hit.transform.GetComponent<ShopRoom>();

            if (door != null)
            {
                clearEventText = false;
                interactEvent.text = "Press E To Spend " + door.cost + " To Open";
                //StartCoroutine(showMessage("Press E To Spend " + door.cost + " To Open", 0.5f));
            }

            if(shop != null)
            {
                //(showMessage("Press E To Open Shop", 0.2f));
                clearEventText = false;
                interactEvent.text = "Press E To Open Shop";
            }

        }

        if (clearEventText)
        {
            interactEvent.text = "";
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator showMessage(string mess, float delay, Text panel)
    {
        panel.text = mess;

        yield return new WaitForSeconds(delay);

        panel.text = "";
    }

    /// <summary>
    /// Open the shop menu
    /// </summary>
    void openShop()
    {
        RaycastHit hit;

        if(Physics.Raycast(pov.transform.position, pov.transform.forward, out hit, range))
        {
            ShopRoom shop = hit.transform.GetComponent<ShopRoom>();

            if(shop != null)
            {
                shopPanel.SetActive(true);
                Cursor.visible = true;

                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }

    void switchWeapons()
    {
        weapons[currentWeapon].SetActive(false);

        currentWeapon += 1;
        currentWeapon = currentWeapon == weapons.Length ? 0 : currentWeapon;

        UnityEngine.Debug.Log(weapons[currentWeapon].GetComponent<GunScript>().own);

        while(!weapons[currentWeapon].GetComponent<GunScript>().own)
        {
            currentWeapon += 1;
            currentWeapon = currentWeapon == weapons.Length ? 0 : currentWeapon;
        }

        weapons[currentWeapon].SetActive(true);
    }

    public void closePauseMenu()
    {
        Time.timeScale = 1;
        outGame = false;
        pausePanel.SetActive(false);
    }
}

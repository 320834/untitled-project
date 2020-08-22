using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public int ammoCost;
    public int umpCost;
    public int akCost;
    public int mfaoCost;

    public GameObject shopPanel;

    public GameObject UMP;
    public GameObject Ak;
    public GameObject mfao;

    public Text ammoCostText;
    public Text umpCostText;
    public Text akCostText;
    public Text mfaoCostText;

    public PlayerInteract playerInteract;

    // Start is called before the first frame update
    void Start()
    {
        if(ammoCostText != null)
        {
            ammoCostText.text = ammoCost.ToString();
        }

        if (umpCostText != null)
        {
            umpCostText.text = umpCost.ToString();
        }

        if (akCostText != null)
        {
            akCostText.text = akCost.ToString();
        }

        if(mfaoCostText != null)
        {
            mfaoCostText.text = mfaoCost.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectAmmo()
    {
        if(GameState.purchase(ammoCost))
        {
            int current = playerInteract.currentWeapon;

            playerInteract.weapons[current].GetComponent<GunScript>().totalAmmo += 60;
        }   
        else
        {
            //Emit not enough credits
            Debug.Log("Not Enough");
        }
    }

    public void selectUMP()
    {
        if (GameState.purchase(umpCost))
        {
            UMP.GetComponent<GunScript>().own = true;
            umpCostText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not Enough");
        }
    }

    public void selectAk()
    {
        if (GameState.purchase(akCost))
        {
            Ak.GetComponent<GunScript>().own = true;
            akCostText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not Enough");
        }
    }

    public void selectMfao()
    {
        if (GameState.purchase(akCost))
        {
            mfao.GetComponent<GunScript>().own = true;
            mfaoCostText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not Enough");
        }
    }

    /// <summary>
    /// Closes shope and resumes the game
    /// </summary>
    public void doneShop()
    {
        shopPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}

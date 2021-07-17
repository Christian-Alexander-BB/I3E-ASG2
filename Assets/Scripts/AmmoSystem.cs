using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public int ammo = 10;
    public int collectedAmmoAmount = 10;
    public Text ammoCountUI;
    public GameObject parentObj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammoCountUI.text = ammo.ToString();

        if(ammo == 0)
        {
            parentObj.GetComponent<PlayerMovement>().hasAmmo = false;
        }

        else if(ammo < 0)
        {
            ammo += 1;
        }


    }
}

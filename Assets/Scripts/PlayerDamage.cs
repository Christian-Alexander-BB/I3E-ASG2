using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerDamage : MonoBehaviour
{
    public GameObject player;
    public Text healthShownInUIText;
    public int playerHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        healthShownInUIText.text = playerHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Zombie")
        {
            Debug.Log("hit");
            playerHealth -= 10;
            healthShownInUIText.text = playerHealth.ToString();
        }
    }
}

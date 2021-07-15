using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerDamage : MonoBehaviour
{
    public PlayerMovement other;
    public GameObject player;
    public Text healthShownInUIText;
    public int playerHealth = 100;
    float timePassed;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        healthShownInUIText.text = playerHealth.ToString();
        timePassed += Time.deltaTime;

        if (playerHealth <= 0)
        {
            other.hideWhenDie();
            other.showWhenDie();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.tag == "Zombie")
        {
                playerHealth -= 10;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Zombie" && timePassed > 0.1)
        {
            
        }
    }
}

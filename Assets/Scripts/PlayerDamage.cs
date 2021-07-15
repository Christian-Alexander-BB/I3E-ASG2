using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerDamage : MonoBehaviour
{
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
        if (playerHealth > 0)
        {
            healthShownInUIText.text = playerHealth.ToString();
            timePassed += Time.deltaTime;
        }
        
        if (playerHealth <= 0)
        {
            healthShownInUIText.text = "0";
            player.GetComponent<PlayerMovement>().showWhenDie();
            player.GetComponent<PlayerMovement>().hideWhenDie();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
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

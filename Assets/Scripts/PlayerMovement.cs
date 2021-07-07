using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject[] uiOnClose;
    public GameObject[] vinylPrompt;
    public GameObject[] vinylPlayerPrompt;
    public GameObject[] vinylAssemble;

    public CharacterController controller;

    public Camera fpsCam;

    public float damage = 10f;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float interactionDistance = 10f;
    public float collectInteractionDistance = 2f;

    public bool vinylFlag = false;
    public bool vinylPlayerFlag = false;

    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    //Christian's variables. I added stuff. 
    public AudioSource shoot;
    public int ammo = 30;

    void Start()
    {
        uiOnClose = GameObject.FindGameObjectsWithTag("ShowOnClose");
        vinylPrompt = GameObject.FindGameObjectsWithTag("ShowOnVinyl");
        vinylPlayerPrompt = GameObject.FindGameObjectsWithTag("ShowOnVinylPlayer");
        vinylAssemble = GameObject.FindGameObjectsWithTag("ShowAssemblePrompt");
        hideClose();
        hideVinylPrompt();
        hideVinylPlayerPrompt();
        hideVinylAssemble();
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            shoot.Play();

            if (ammo == 0)
            {
                Debug.Log("No ammo");
            }

            else
            {
                ammo -= 1;
            }
        }

        Collect();
    }

    void Shoot()
    {
        RaycastHit result;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out result, interactionDistance))
        {
            Debug.Log(result.transform.name);

            Target target = result.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    void Collect()
    {
        RaycastHit result;
        Debug.DrawLine(fpsCam.transform.position, fpsCam.transform.position + fpsCam.transform.forward * collectInteractionDistance);
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out result, collectInteractionDistance))
        {
            if (result.transform.name == "Collectible")
            {
                showClose();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    result.transform.gameObject.SetActive(false);
                    hideClose();
                }
            }

            if (result.transform.name == "Vinyl")
            {
                showVinylPrompt();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    vinylFlag = true;
                    result.transform.gameObject.SetActive(false);
                    hideVinylPrompt();
                }
            }

            else if (result.transform.name != "Vinyl")
            {
                hideVinylPrompt();
            }

            if (result.transform.name == "Vinyl Player")
            {
                showVinylPlayerPrompt();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    vinylPlayerFlag = true;
                    result.transform.gameObject.SetActive(false);
                    hideVinylPlayerPrompt();
                }
            }

            else if (result.transform.name != "Vinyl Player")
            {
                hideVinylPlayerPrompt();
            }

        }

        if (vinylFlag && vinylPlayerFlag)
        {
            showVinylAssemble();
            //if (Input.GetKeyDown(KeyCode.Y))
            {
                //hideVinylAssemble();
            }
        }

    }

   void hideClose()
   {
       foreach (GameObject s in uiOnClose)
       {
           s.SetActive(false);
       }
   }

   void showClose()
   {
       foreach (GameObject s in uiOnClose)
       {
           s.SetActive(true);
       }
   }

    void showVinylPrompt()
    {
        foreach (GameObject s in vinylPrompt)
        {
            s.SetActive(true);
        }
    }

    void hideVinylPrompt()
    {
        foreach (GameObject s in vinylPrompt)
        {
            s.SetActive(false);
        }
    }

    void showVinylPlayerPrompt()
    {
        foreach (GameObject s in vinylPlayerPrompt)
        {
            s.SetActive(true);
        }
    }

    void hideVinylPlayerPrompt()
    {
        foreach (GameObject s in vinylPlayerPrompt)
        {
            s.SetActive(false);
        }
    }

    void showVinylAssemble()
    {
        foreach (GameObject s in vinylAssemble)
        {
            s.SetActive(true);
        }
    }

    void hideVinylAssemble()
    {
        foreach (GameObject s in vinylAssemble)
        {
            s.SetActive(false);
        }
    }

}

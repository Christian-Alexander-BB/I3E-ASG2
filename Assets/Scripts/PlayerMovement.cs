using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject[] vinylPrompt;
    public GameObject[] findVinyl;
    public GameObject[] vinylPlayerPrompt;
    public GameObject[] vinylAssemble;
    public GameObject[] dieShowObjects;
    public GameObject[] dieHideObjects;
    public GameObject[] ammoObjects;
    public GameObject[] ammoPrompt;

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
    public AudioSource vinylSong;
    //public int ammo = 30;
    bool allowShowVinylAssemble = true;
    public bool hasAmmo = true;
    public GameObject ammoSystem;

    void Start()
    {
        vinylPrompt = GameObject.FindGameObjectsWithTag("ShowOnVinyl");
        vinylPlayerPrompt = GameObject.FindGameObjectsWithTag("ShowOnVinylPlayer");
        vinylAssemble = GameObject.FindGameObjectsWithTag("ShowAssemblePrompt");
        findVinyl = GameObject.FindGameObjectsWithTag("ShowFindVinyl");
        dieShowObjects = GameObject.FindGameObjectsWithTag("ShowWhenDie");
        dieHideObjects = GameObject.FindGameObjectsWithTag("HideWhenDie");
        ammoObjects = GameObject.FindGameObjectsWithTag("AmmoBox");
        ammoPrompt = GameObject.FindGameObjectsWithTag("ShowAmmoPrompt");
        hideVinylPrompt();
        hideVinylPlayerPrompt();
        hideVinylAssemble();
        hideAmmoPrompt();
        showFindVinyl();
        showWhenDie();
        hideBeforeDie();
        
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

        if (Input.GetButtonDown("Fire1") && hasAmmo)
        {
            Shoot();
            ammoSystem.GetComponent<AmmoSystem>().ammo -= 1;

           //if (ammo == 0)
           //{
           //    Debug.Log("No ammo");
           //}
           //
           //else
           //{
           //    ammo -= 1;
           //}
        }

        Collect();
    }

    void Shoot()
    {
        RaycastHit result;
        shoot.Play();

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out result, interactionDistance))
        {
            Debug.Log("fudgecakes" + result.transform.name);

            Target target = result.transform.GetComponent<Target>();
            if (target != null)
            {
                Debug.Log("Damage Damage Damage!");
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
            if (result.transform.name == "Vinyl")
            {
                hideFindVinyl();
                showVinylPrompt();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    vinylFlag = true;
                    result.transform.gameObject.SetActive(false);
                    hideVinylPrompt();
                    showFindVinyl();
                }
            }

            else if (result.transform.name != "Vinyl")
            {
                hideVinylPrompt();
                hideVinylPlayerPrompt();
                hideAmmoPrompt();
                if (vinylFlag == false && vinylPlayerFlag == false)
                {
                    showFindVinyl();
                }
            }

            if (result.transform.name == "Vinyl Player")
            {
                hideFindVinyl();
                showVinylPlayerPrompt();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    vinylPlayerFlag = true;
                    result.transform.gameObject.SetActive(false);
                    hideVinylPlayerPrompt();
                    showFindVinyl();
                }
            }

            if (result.transform.name == "ammo_box_w_bullets")
            {
                hideFindVinyl();
                showAmmoPrompt();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ammoSystem.GetComponent<AmmoSystem>().ammo += 10;
                    hasAmmo = true;
                    hideAmmoBox();
                    hideAmmoPrompt();
                }
            }
        }

        if (vinylFlag && vinylPlayerFlag)
        {
            if (allowShowVinylAssemble)
            {
                hideFindVinyl();
                showVinylAssemble();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                allowShowVinylAssemble = false;
                vinylSong.Play();
                hideVinylAssemble();
            }
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

    void showFindVinyl()
    {
        foreach (GameObject s in findVinyl)
        {
            s.SetActive(true);
        }
    }

    void hideFindVinyl()
    {
        foreach (GameObject s in findVinyl)
        {
            s.SetActive(false);
        }
    }

    public void showWhenDie()
    {
        foreach (GameObject s in dieShowObjects)
        {
            s.SetActive(true);
        }
    }

    void hideBeforeDie()
    {
        foreach (GameObject s in dieShowObjects)
        {
            s.SetActive(false);
        }
    }

    public void hideWhenDie()
    {
        foreach (GameObject s in dieHideObjects)
        {
            s.SetActive(false);
        }
    }

    void showAmmoPrompt()
    {
        foreach (GameObject s in ammoPrompt)
        {
            s.SetActive(true);
        }
    }

    void hideAmmoPrompt()
    {
        foreach (GameObject s in ammoPrompt)
        {
            s.SetActive(false);
        }
    }

    void hideAmmoBox()
    {
        foreach (GameObject s in ammoObjects)
        {
            s.SetActive(false);
        }
    }

}

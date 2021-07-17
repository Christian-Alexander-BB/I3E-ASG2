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
    public GameObject[] ammoPrompt;
    public GameObject[] keycardPrompt;

    //door thing @christian
    public GameObject[] acceptedPrompt;
    public GameObject[] rejectedPrompt;
    //

    public CharacterController controller;

    public Camera fpsCam;
    public LayerMask collectiblesMask;
    public int progress = 0;
    public Text progressText;
    public Text deathProgressText;

    public float damage = 10f;

    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float interactionDistance = 10f;
    public float collectInteractionDistance = 2f;

    public bool vinylFlag = false;
    public bool vinylPlayerFlag = false;
    public bool keycardFlag = false;

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
    public bool collectAmmoFlag = false;
    public GameObject ammoSystem;
    public bool allowMovement = true;

    void Start()
    {
        vinylPrompt = GameObject.FindGameObjectsWithTag("ShowOnVinyl");
        vinylPlayerPrompt = GameObject.FindGameObjectsWithTag("ShowOnVinylPlayer");
        vinylAssemble = GameObject.FindGameObjectsWithTag("ShowAssemblePrompt");
        findVinyl = GameObject.FindGameObjectsWithTag("ShowFindVinyl");
        dieShowObjects = GameObject.FindGameObjectsWithTag("ShowWhenDie");
        dieHideObjects = GameObject.FindGameObjectsWithTag("HideWhenDie");
        ammoPrompt = GameObject.FindGameObjectsWithTag("ShowAmmoPrompt");
        keycardPrompt = GameObject.FindGameObjectsWithTag("KeycardPrompt");

        //door accept thing @christian
        acceptedPrompt = GameObject.FindGameObjectsWithTag("ShowOnAccepted");
        rejectedPrompt = GameObject.FindGameObjectsWithTag("ShowOnRejected");
        //

        hideVinylPrompt();
        hideVinylPlayerPrompt();
        hideVinylAssemble();
        hideAmmoPrompt();
        hideKeycardPrompt();
        showFindVinyl();
        showWhenDie();
        hideBeforeDie();
        
    }

    // Update is called once per frame
    void Update()
    {
        //deathProgressText.text = "Progress\n" + progress + " Percent";

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.anyKey && allowMovement)
        {
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
        }

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
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out result, collectInteractionDistance, collectiblesMask))
        {
            if (result.transform.name == "Vinyl")
            {
                hideFindVinyl();
                showVinylPrompt();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    progress += 15;
                    progressText.text = "Progress\n" + progress + " Percent";
                    vinylFlag = true;
                    result.transform.gameObject.SetActive(false);
                    hideVinylPrompt();
                    showFindVinyl();
                }
            }

            if (result.transform.name == "Vinyl Player")
            {
                hideFindVinyl();
                showVinylPlayerPrompt();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    progress += 15;
                    progressText.text = "Progress\n" + progress + " Percent";
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
                    progress += 15;
                    progressText.text = "Progress\n" + progress + " Percent";
                    ammoSystem.GetComponent<AmmoSystem>().ammo += 10;
                    hasAmmo = true;
                    collectAmmoFlag = true;
                    result.transform.gameObject.SetActive(false);
                    hideAmmoPrompt();
                }
            }

            if (result.transform.name == "keycard")
            {
                hideFindVinyl();
                showKeycardPrompt();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    progress += 15;
                    progressText.text = "Progress\n" + progress + " Percent";
                    keycardFlag = true;
                    result.transform.gameObject.SetActive(false);
                    hideKeycardPrompt();
                    showFindVinyl();
                }
            }
        }

        else
        {
            hideVinylPrompt();
            hideVinylPlayerPrompt();
            hideAmmoPrompt();
            hideKeycardPrompt();
            if (vinylFlag == false && vinylPlayerFlag == false && collectAmmoFlag == false && keycardFlag == false)
            {
                showFindVinyl();
            }
        }

        if (vinylFlag && vinylPlayerFlag)
        {
            if (allowShowVinylAssemble)
            {
                hideFindVinyl();
                showVinylAssemble();
                Time.timeScale = 0;
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Time.timeScale = 1;
                allowShowVinylAssemble = false;
                vinylSong.Play();
                hideVinylAssemble();
            }
        }

    }

    void OpenDoor()
    {
        //door thing @christian
        if (keycardFlag)
        {
            // open door stuff thing @christian
            showAcceptedPrompt();
        }

        else if (keycardFlag == false)
        {
            showRejectedPrompt();
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

    void showKeycardPrompt()
    {
        foreach (GameObject s in keycardPrompt)
        {
            s.SetActive(true);
        }
    }

    void hideKeycardPrompt()
    {
        foreach (GameObject s in keycardPrompt)
        {
            s.SetActive(false);
        }
    }


    //door thing @christian
    public void showAcceptedPrompt()
    {
        foreach (GameObject s in acceptedPrompt)
        {
            s.SetActive(true);
        }
    }

    public void showRejectedPrompt()
    {
        foreach (GameObject s in rejectedPrompt)
        {
            s.SetActive(true);
        }
    }
    //

}

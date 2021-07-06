using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject[] uiOnClose;

    public CharacterController controller;

    public Camera fpsCam;

    public float damage = 10f;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float interactionDistance = 10f;
    public float collectInteractionDistance = 0.5f;

    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    void Start()
    {
        uiOnClose = GameObject.FindGameObjectsWithTag("ShowOnClose");
        hideClose();
        
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

            else
            {
                hideClose();
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class DoorInteractions : MonoBehaviour
{
    //Public variables
    public Camera playerCam;
    public float maxRayDistanceToVShopDoor = 2f;
    public Image blackScreenImage;
    public Text[] allText;
    public Color defaultColor = new Color32(50, 50, 50, 255);
    public float fadeAmount = 0.1f;
    public GameObject player;

    public Animator animator;

    public string outsideBuildingPrompt = "Press E To Enter";
    public string insideBuildingPrompt = "Press E To Leave";
    public string noKeyCardPrompt = "No Keycard";

    //Private variables
    private RaycastHit hitObject;
    private int framesPassedSinceRayHit = 0;
    private int framesPassedWithoutRayHit = 0;
    private bool insideBuilding = true;
    private string prompt;
    private Color invisible = new Color32(0, 0, 0, 0);
    private string whichDoor;

    // Start is called before the first frame update
    void Start()
    {
        //opacity = (byte)(blackScreenImage.color.a * 255);
        prompt = insideBuildingPrompt;
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hitObject, maxRayDistanceToVShopDoor) && 
            hitObject.transform.gameObject.tag == "Door")
        {
            whichDoor = hitObject.transform.gameObject.name;
            framesPassedSinceRayHit += 1;
            framesPassedWithoutRayHit = 0;

            if (framesPassedSinceRayHit == 1)
            {
                StartCoroutine("ActivateExitOrEnterPrompt");
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine("FadeToBlack");
            }
        }

        else
        {
            framesPassedSinceRayHit = 0;
            framesPassedWithoutRayHit += 1;

            if (framesPassedWithoutRayHit == 1)
            {
                StartCoroutine("DeactivateExitOrEnterPrompts");
            }

        }
    }

    IEnumerator ActivateExitOrEnterPrompt()
    {
        for(int i = 0; i < allText.Length; ++i)
        {
            if (allText[i].gameObject.name == "Prompt")
            {
                allText[i].text = prompt;
                allText[i].color = defaultColor;
            }

            else
            {
                allText[i].color = invisible;

            }
        }

        yield return null;
    }

    IEnumerator DeactivateExitOrEnterPrompts()
    {
        for(int i = 0; i < allText.Length; ++i)
        {
            if (allText[i].gameObject.name == "Prompt")
            {
                allText[i].color = invisible;
            }

            else
            {
                allText[i].color = defaultColor;
            }
        }

        yield return null;
    }

    IEnumerator FadeToBlack()
    {
        if(whichDoor == "DeconChamberDoor" && !gameObject.GetComponent<PlayerMovement>().keycardFlag)
        {
            for (int i = 0; i < allText.Length; ++i)
            {
                if (allText[i].gameObject.name == "NoKeycardPrompt")
                {
                    allText[i].color = defaultColor;
                    allText[i].text = noKeyCardPrompt;
                }

                else
                {
                    allText[i].color = invisible;
                }
            }

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < allText.Length; ++i)
            {
                if (allText[i].gameObject.name == "NoKeycardPrompt")
                {
                    allText[i].color = invisible;
                    allText[i].text = "";
                }

                else
                {
                    allText[i].color = defaultColor;
                }
            }

            yield break;
        }

        player.GetComponent<PlayerMovement>().allowMovement = false;

        animator.SetBool("AnimationDone", false);
        animator.SetBool("ChangeLocation", true);
        animator.SetBool("LocationChanged", false);

        yield return new WaitForSeconds(1.1f);

        if (whichDoor == "VinylShopDoor")
        {
            EnterOrExitVinylShop();
        }

        if (whichDoor == "DeconChamberDoor")
        {
            EnterOrExitDeconChamber();
        }

        yield return new WaitForSeconds(0.1f);

        animator.SetBool("LocationChanged", true);
        animator.SetBool("ChangeLocation", false);
        animator.SetBool("AnimationDone", true);

        player.GetComponent<PlayerMovement>().allowMovement = true;


        yield return null;
    }

    void EnterOrExitVinylShop()
    {
        if (insideBuilding)
        {
            player.transform.position = new Vector3(1.5f, 1f, 1.5f);
            prompt = outsideBuildingPrompt;
            insideBuilding = false;
        }

        else
        {
            player.transform.position = new Vector3(1.5f, 1f, -0.8f);
            prompt = insideBuildingPrompt;
            insideBuilding = true;
        }
    }

    void EnterOrExitDeconChamber()
    {
        if (insideBuilding)
        {
            player.transform.position = new Vector3(50.72f, 1f, -5.75f);
            prompt = outsideBuildingPrompt;
            insideBuilding = false;
        }

        else
        {
            player.transform.position = new Vector3(50.72f, 1f, -7.5f);
            prompt = insideBuildingPrompt;
            insideBuilding = true;
        }
    }
}

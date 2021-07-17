using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorInteractions : MonoBehaviour
{
    //Public variables
    public Camera playerCam;
    public float maxRayDistanceToVShopDoor = 2f;
    public Image blackScreenImage;
    public Text[] allText;
    public Color defaultColor = new Color32(50, 50, 50, 255);
    public float fadeAmount = 0.1f;

    //Private variables
    private RaycastHit hitObject;
    private int framesPassedSinceRayHit = 0;
    private int framesPassedWithoutRayHit = 0;
    private bool insideBuilding = true;
    private string insideBuildingPrompt = "Press E To Leave";
    private string outsideBuildingPrompt = "Press E To Enter";
    private Color invisible = new Color32(0, 0, 0, 0);
    private byte opacity = 0;

    // Start is called before the first frame update
    void Start()
    {
        //opacity = (byte)(blackScreenImage.color.a);
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hitObject, maxRayDistanceToVShopDoor) && 
            hitObject.transform.gameObject.tag == "Door")
        {
            framesPassedSinceRayHit += 1;
            framesPassedWithoutRayHit = 0;

            if (framesPassedSinceRayHit == 1)
            {
                StartCoroutine("ActivateExitOrEnterPrompt");
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine("FadeToBlack");

                if (insideBuilding)
                {
                    insideBuilding = false;
                }

                else
                {
                    insideBuilding = true;
                }
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
                if (insideBuilding)
                {
                    allText[i].text = insideBuildingPrompt;
                }

                else
                {
                    allText[i].text = outsideBuildingPrompt;
                }

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
        while (opacity < 255)
        {
            opacity += (byte)(fadeAmount);
            blackScreenImage.color = new Color32((byte)(blackScreenImage.color.r * 255),
                                                 (byte)(blackScreenImage.color.g * 255),
                                                 (byte)(blackScreenImage.color.b * 255),
                                                 opacity);
        }

        yield return null;
    }
}

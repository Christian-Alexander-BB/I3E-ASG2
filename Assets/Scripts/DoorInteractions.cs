using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorInteractions : MonoBehaviour
{
    public Camera playerCam;
    public float maxRayDistanceToVShopDoor = 2f;
    public GameObject blackScreenImage;
    public string exitVinylShopPrompt = "Press F to leave";
    public Text[] allText;
    
    [SerializeField]
    private RaycastHit hitObject;

    [SerializeField]
    private int framesPassedSinceRayHit = 0;

    [SerializeField]

    // Start is called before the first frame update
    void Start()
    {
        foreach(Text text in allText)
        {
            if(text.gameObject.name == "ExitVinylShopPrompt")
            {
                text.text = exitVinylShopPrompt;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hitObject, maxRayDistanceToVShopDoor) && 
            hitObject.transform.gameObject.name == "VinylShopDoor")
        {
            Debug.Log("1 thing");
            framesPassedSinceRayHit += 1;

            if (framesPassedSinceRayHit == 1)
            {
                StartCoroutine("ActivateExitVinylShopPrompt");
            }

            Debug.Log("2 things");
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                blackScreenImage.SetActive(true);
            }
        }

        else
        {
            framesPassedSinceRayHit = 0;
        }
    }

    IEnumerator ActivateExitVinylShopPrompt()
    {
        for(int i = 0; i < allText.Length; ++i)
        {
            Debug.Log("Text");
            if (allText[i].gameObject.name != "ExitVinylShopPrompt")
            {
                allText[i].color = new Color32(0, 0, 0, 0);
            }

            else
            {
                allText[i].color = new Color32(50, 50, 50, 255);
            }
        }

        yield return null;
    }

    IEnumerator DeactivateExitVinylShopPrompt()
    {
        for(int i = 0; i < allText.Length; ++i)
        {

        }

        yield return null;
    }
}

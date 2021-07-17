using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorInteractions : MonoBehaviour
{
    public Camera playerCam;
    public float maxRayDistanceToVShopDoor = 2f;
    public GameObject blackScreenImage;
    public string exitPrompt = "Press F to leave";
    public GameObject[] allText;
    
    [SerializeField]
    private RaycastHit hitObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hitObject, maxRayDistanceToVShopDoor) && 
            hitObject.transform.gameObject.name == "vinylShopDoor")
        {
            StartCoroutine("ActivateExitVinylShopPrompt");
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                blackScreenImage.SetActive(true);
            }
        }
    }

    IEnumerator ActivateExitVinylShopPrompt()
    {
        foreach (GameObject text in allText)
        {
            Debug.Log("Text");
            if (text.name == "ExitVinylShopPrompt")
            {
                text.SetActive(true);
            }

            else
            {
                Debug.Log("Close This");
                text.SetActive(false);
            }
        }

        yield return new WaitForSeconds(1f);
    }
}

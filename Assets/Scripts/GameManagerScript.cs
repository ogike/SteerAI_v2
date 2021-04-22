using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is the script thatcan be accesed from anywhere in the scene
 * Use this if you want to handle GameStage stuff, or access the player/camera from different scripts
 *      Especially because GetComponent is slow, accesing player components from this script is ideal 
 */

public class GameManagerScript : MonoBehaviour
{

    //make this instance of the GameManagerScript static, so this will be a static script basically
    //which means it can be acesses from anywhere
    //the get/set thing is so everyone can access this, but only this script can set this static variable (which happens in Awake)
    public static GameManagerScript Instance { get; private set; } //basically make this a singleton

    public GameObject playerObject; //need to be set manually
    public int playerZPosition = 0;

    [HideInInspector] public Transform playerTransform;


    //Vector3 lerpHelperVelocity = Vector3.zero; //this is for the Vector3.SmoothDamp in the CameraTranstion(), unity handles it

    //Awake is called before start
    private void Awake()
    {
        Instance = this;

        playerTransform = playerObject.GetComponent<Transform>();
    }


    public void SetPlayerPosition(Vector3 newPos)
    {
        //basically teleport
        playerTransform.position = new Vector3(newPos.x, newPos.y, playerZPosition);
        //fancy stuff goes here
    }
}

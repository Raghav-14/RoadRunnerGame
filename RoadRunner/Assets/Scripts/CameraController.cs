using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform lookAt; //player position 
    private Vector3 startingOffset; //distance between camera and player
    private Vector3 moveTransfrom;

    //variables for starting transitions
    private float transition = 0.0f;
    private float cameraTransitionDuration = 1.0f;
    private Vector3 cameraOffset = new Vector3 (0, 5, 14); 

    // Start is called before the first frame update
    void Start()
    {
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        startingOffset = transform.position - lookAt.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveTransfrom = lookAt.position + startingOffset;
        //for X 
        moveTransfrom.x = 0; //want camera to keep center always
        //for y 
        moveTransfrom.y = Mathf.Clamp(moveTransfrom.y, 3.0f, 5.0f);

        if (transition > 1.0f)
        {
            transform.position = moveTransfrom;
        }
        else
        {
            //Lerp Interpolate between 2 points which are first 2 parameters
            //starting animation 
            transform.position = Vector3.Lerp(moveTransfrom + cameraOffset,
                 moveTransfrom, //moving position
                transition); //between 0 to 1
            transition += Time.deltaTime * 1 / cameraTransitionDuration; // increase transition value  
            transform.LookAt(lookAt.position + Vector3.up); //Looking at player while transitioning
        }

    }
}

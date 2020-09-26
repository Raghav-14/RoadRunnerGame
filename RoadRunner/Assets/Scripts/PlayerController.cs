using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController charController;
    public float charSpeed = 10.0f;
    private Vector3 moveVector; //char moving variable
    private float verticleVelocity = 0.0f;
    private float charGravity = 1.0f;
    private float sideOffsetToMove = 6.0f; // left and right movement 

    private float cameraTransitionDuration = 3.0f; //starting transition

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time < cameraTransitionDuration)
        {
            //While there is starting transition of camera then we dont move player left and right  
            charController.Move(Vector3.forward * Time.deltaTime * charSpeed);
            return;
        }
        //reset to zero
        moveVector = Vector3.zero;
        //for left and right
        moveVector.x = Input.GetAxisRaw("Horizontal") * sideOffsetToMove;
       // moveVector.x = Mathf.Clamp(moveVector.x, -3, 3);
        //for up and down
        if(charController.isGrounded)
        {
            verticleVelocity = -0.5f;
        }
        else
        {
            verticleVelocity -= charGravity * Time.deltaTime;
        }
        moveVector.y = verticleVelocity;
        //for forward and backward
        moveVector.z = charSpeed;

        charController.Move(moveVector * Time.deltaTime );
    }
}

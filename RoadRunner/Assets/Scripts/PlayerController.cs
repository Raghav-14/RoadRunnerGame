using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController charController;
    public float charSpeed = 6.0f;
    private Vector3 moveVector; //char moving variable
    private float verticleVelocity = 0.0f;
    private float charGravity = 13.0f;
    private float sideOffsetToMove = 30.0f; // left and right movement 
    private float jumpValue = 7.0f;

    private float cameraTransitionDuration = 3.0f; //starting transition

    public Animator anim;

    //swipe control
    //these two will help us know what exactly is a swipe
    public float maxSwipeTime;//0.5
    public float minSwipeDistance;//100


    //these three will help us know how long did our swipe took
    private float startTime;
    private float endTime;
    private float swipeTime;//this will be compared with maxTime;


    //these three will help us know how long the swipe is
    private Vector2 swipeStartPos;
    private Vector2 swipeEndPos;
    private float swipeDistance;//this will be compared with minSwipeDistance;


    //death
    private bool isDead = false;

    //Lives
    private int lives = 0;
    private int maxLives = 3;
    public Text livesText;


    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        anim.SetBool("Run",true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Time.time < cameraTransitionDuration)
        {
            //    //While there is starting transition of camera then we dont move player left and right  
            charController.Move(Vector3.forward * Time.deltaTime * charSpeed);
            return;
        }
        //reset to zero
        moveVector = Vector3.zero;
        //for left and right and jump and slide
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    anim.SetTrigger("LeftMove");
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    anim.SetTrigger("RightMove");
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    anim.SetTrigger("Slide");
        //}

        //if(Input.GetMouseButton(0))
        //{
        //    if(Input.mousePosition.x > Screen.width/2)
        //    {
        //        moveVector.x = sideOffsetToMove;
        //        anim.SetTrigger("RightMove");
        //    }
        //    else
        //    {
        //        moveVector.x = -sideOffsetToMove;
        //        anim.SetTrigger("LeftMove");
        //    }
        //}
       // moveVector.x = Input.GetAxisRaw("Horizontal") * sideOffsetToMove;
        //for up and down
        if (charController.isGrounded)
        {
            verticleVelocity = -0.5f;
            //if(Input.GetKeyDown(KeyCode.Space))
            //{
            //    verticleVelocity = jumpValue;
            //    anim.SetBool("Jump", true);
            //    anim.SetBool("Run", false);
            //}

            SwipeTest();
        }
        else
        {
            verticleVelocity -= charGravity * Time.deltaTime;
            anim.SetBool("Jump", false);
            anim.SetBool("Run", true);
        }
        moveVector.y = verticleVelocity;
        //for forward and backward
        moveVector.z = charSpeed;

        charController.Move(moveVector * Time.deltaTime);
    }


    //Thiis function is called in scoreController
    public void setSpeedToNextLevel(float difficultyValue)
    {
        charSpeed = 6.0f + difficultyValue;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(lives < maxLives)
        {
            if (hit.gameObject.name == "oldCar" || hit.gameObject.name == "Bike" || hit.gameObject.name == "JumpObstacal"
               || hit.gameObject.name == "oil-truck_fbx" || hit.gameObject.name == "oldCar" || hit.gameObject.name == "barrier")
            {
                maxLives = maxLives - 1;
                livesText.text = maxLives.ToString();
                charController.transform.position = transform.position - new Vector3(0.0f, -0.06999993f, -30.0f);
            }
        }
        else
        {
            Death();
        }
    }

    private void Death()
    {
       isDead = true;
       GetComponent<ScoreController>().OnDeath();
    }


    void SwipeTest()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;//this will see when we started touching the screen
                swipeStartPos = touch.position;//where we have started touching the screen
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;//the time when we left th screen
                swipeEndPos = touch.position;//the position when we left the screen

                swipeTime = endTime - startTime;//this will calculate how long our swip took
                swipeDistance = (swipeEndPos - swipeStartPos).magnitude; //this will calculate how long our swipe is

                if (swipeTime < maxSwipeTime && swipeDistance > minSwipeDistance)
                {//here if we swipe fast and long enough then it will be a swipe
                    SwipeControl();
                }
            }
        }
    }

    void SwipeControl()
    {
        Vector2 distance = swipeEndPos - swipeStartPos;
        float xDistance = Mathf.Abs(distance.x);
        float yDistance = Mathf.Abs(distance.y);
        if (xDistance > yDistance)
        {

            Debug.Log("horizontal swipe");
            if (distance.x > 0)
            {
                moveVector.x = sideOffsetToMove ;
                anim.SetTrigger("RightMove");
                //FlipAndMove();
            }
            else if (distance.x < 0 )
            {
                //your swiping left
                //FlipAndMove();
                moveVector.x = -sideOffsetToMove ;
                anim.SetTrigger("LeftMove");
            }
        }
        if (xDistance < yDistance)//if you are swiping up or down
        {
            Debug.Log("vertical swipe");
            if (distance.y > 0)
            {
                //your swiping up
                verticleVelocity = jumpValue;
                anim.SetBool("Jump", true);
                anim.SetBool("Run", false);
                //playerRB.velocity = Vector2.up * jumpHeight * Time.deltaTime;
            }
            else if (distance.y < 0)
            {
                // your swiping down
                anim.SetTrigger("Slide");
            }

        }
    }
}


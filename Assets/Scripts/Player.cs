using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    private Vector2 gravity;
    private Vector2 jumpForce;
    private Vector2 movement;

    //player handler variables
    private bool jumping = true;
    private bool canRotate = false;

    //These boolean variables are local containers for direction of gravity
    private bool gravX = false; //Does gravity work in the x direction
    private bool gravPositive = false; //Is gravity positive

    public float magnitude; //Intensity of the gravity
    public float jumpMag; //Intensity of the jump force
    public float moveSpeed; //Intensity of direct movement force

    public static Player instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        magnitude = instance.magnitude;
        jumpMag = instance.jumpMag;
        moveSpeed = instance.moveSpeed;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //******************
        // ROTATION HANDLER note: this code can (and ought to) be simplified | consider using a single function for the rotation controls
        //******************
        if (!gravX && !gravPositive) //GRAVITY DOWN     (zero degrees) [Negative Y Axis]
        {
            // Orientation
            transform.rotation = Quaternion.Euler(0, 0, 0);                         //Rotates the player right-side up
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))                 //Locks AxisRaw inputs to "A" and "D" keys only (ensures arrow keys cannot be used)
                movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0);          //Sets movement axis perpendicular to the ground (right-side up)
            else movement = Vector2.zero;                                           //If no horizontal input is received the movement vector is zero

            // Rotation Controls
            if (canRotate)                                                          //When enabled
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))     //Rotation key Left
                { gravX = true; gravPositive = false; canRotate = false; }                  //      Turn to GRAVITY LEFT position | Disable rotation until player touches the ground
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.E))    //Rotation key Right
                { gravX = true; gravPositive = true; canRotate = false; }                   //      Turn to GRAVITY RIGHT position | Disable rotation until player touches the ground

                //Update the global gravity
                GameController.instance.gravX = gravX;
                GameController.instance.gravPositive = gravPositive;
            }

        }
        if (gravX && gravPositive) //GRAVITY RIGHT      ( 90* degrees) [Positive X Axis]
        {
            // Orientation
            transform.rotation = Quaternion.Euler(0, 0, 90);                        //Rotates the player to the right side
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))                 //Locks AxisRaw inputs to "A" and "D" keys only (ensures arrow keys cannot be used)
                movement = new Vector2(0, Input.GetAxisRaw("Horizontal"));          //Sets movement axis perpendicular to the right side
            else movement = Vector2.zero;                                           //If no horizontal input is received the movement vector is zero

            // Rotation Controls
            if (canRotate)                                                          //When enabled
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))     //Rotation key Left
                { gravX = false; gravPositive = false; canRotate = false; }                 //      Turn to GRAVITY DOWN position | Disable rotation until player touches the ground
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.E))    //Rotation key Right
                { gravX = false; gravPositive = true; canRotate = false; }                  //      Turn to GRAVITY UP position | Disable rotation until player touches the ground

                //Update the global gravity
                GameController.instance.gravX = gravX;
                GameController.instance.gravPositive = gravPositive;
            }
        }
        if (!gravX && gravPositive) //GRAVITY UP        (180* degrees) [Positive Y Axis]
        {
            // Orientation
            transform.rotation = Quaternion.Euler(0, 0, 180);                       //Rotates the player upside down
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))                 //Locks AxisRaw inputs to "A" and "D" keys only (ensures arrow keys cannot be used)
                movement = new Vector2((Input.GetAxisRaw("Horizontal") * -1), 0);   //Sets movement axis perpendicular to the ceiling (up-side down)
            else movement = Vector2.zero;                                           //If no horizontal input is received the movement vector is zero

            // Rotation Controls
            if (canRotate)                                                          //When enabled
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))     //Rotation key Left
                { gravX = true; gravPositive = true; canRotate = false; }                   //      Turn to GRAVITY RIGHT position | Disable rotation until player touches the ground
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.E))    //Rotation key Right
                { gravX = true; gravPositive = false; canRotate = false; }                  //      Turn to GRAVITY LEFT position | Disable rotation until player touches the ground

                //Update the global gravity
                GameController.instance.gravX = gravX;
                GameController.instance.gravPositive = gravPositive;
            }
        }    
        if (gravX && !gravPositive) //GRAVITY LEFT      (270* degrees) [Negative X Axis]
        {
            // Orientation
            transform.rotation = Quaternion.Euler(0, 0, -90);                       //Rotates the player to the left side
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))                 //Locks AxisRaw inputs to "A" and "D" keys only (ensures arrow keys cannot be used)
                movement = new Vector2(0, (Input.GetAxisRaw("Horizontal") * -1));   //Sets movement axis perpendicular to the left side
            else movement = Vector2.zero;                                           //If no horizontal input is received the movement vector is zero

            // Rotation Controls
            if (canRotate)                                                          //When enabled
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))     //Rotation key Left
                { gravX = false; gravPositive = true; canRotate = false; }                  //      Turn to GRAVITY UP position | Disable rotation until player touches the ground
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.E))    //Rotation key Right
                { gravX = false; gravPositive = false; canRotate = false; }                 //      Turn to GRAVITY DOWN position | Disable rotation until player touches the ground

                //Update the global gravity
                GameController.instance.gravX = gravX;
                GameController.instance.gravPositive = gravPositive;
            }
        }    

        //*********
        // GRAVITY
        //*********

        //Keep the most up-to-date gravity settings from GameController instance
        gravX = GameController.instance.gravX;
        gravPositive = GameController.instance.gravPositive;

        //Determine the direction of gravity on its axis
        if (gravPositive) { magnitude = Mathf.Abs(magnitude); } //magnitude is a positive multiplier
        else { magnitude = ((Mathf.Abs(magnitude)) * -1); }     //magnitude is a negative multiplier

        //Determine the axis of gravity
        if (gravX) { gravity = new Vector2(magnitude, 0); }
        else { gravity = new Vector2(0, magnitude); }

        rb2d.AddForce(gravity); //Force of gravity is applied to player physics

        //**********
        // CONTROLS
        //**********

        //Jump
        if (Input.GetKeyDown(KeyCode.W) && !jumping)
        {
            //Determine the direction of the jump against gravity axis
            if (gravPositive) { jumpMag = ((Mathf.Abs(jumpMag)) * -1); }    //when gravity is positive jump is negative
            else { jumpMag = Mathf.Abs(jumpMag); }                          //when gravity is negative jump is positive

            //Match the jump axis to the axis of gravity
            if (gravX) { jumpForce = new Vector2(jumpMag, 0); }
            else { jumpForce = new Vector2(0, jumpMag); }

            rb2d.AddForce(jumpForce); //Jump Force is applied to player physics
        }

        //Move
        transform.Translate(movement * moveSpeed * Time.smoothDeltaTime, Space.World); //Rotation handler gets input [See Above]

    }

    //The trigger is on the bottom of the player
    private void OnTriggerStay2D(Collider2D collision)
    {
        jumping = false;
        canRotate = true; //Rotation is enabled when the player touches the ground at least once
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        jumping = true;
    }
}

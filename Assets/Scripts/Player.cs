using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    private Vector2 gravity;
    private Vector2 jumpForce;
    private Vector2 movement;

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

    }

    // Update is called once per frame
    void Update()
    {
        //*********
        // ROTATION
        //*********
        if (!gravX && !gravPositive) { transform.rotation = Quaternion.Euler(0, 0, 0);   movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0); }           //Gravity Down     (zero degrees) [Negative Y Axis]
        if (gravX && gravPositive)   { transform.rotation = Quaternion.Euler(0, 0, 90);  movement = new Vector2(0, Input.GetAxisRaw("Horizontal")); }           //Gravity Right    ( 90* degrees) [Positive X Axis]
        if (!gravX && gravPositive)  { transform.rotation = Quaternion.Euler(0, 0, 180); movement = new Vector2((Input.GetAxisRaw("Horizontal") * -1), 0); }    //Gravity Up       (180* degrees) [Positive Y Axis]
        if (gravX && !gravPositive)  { transform.rotation = Quaternion.Euler(0, 0, -90); movement = new Vector2(0, (Input.GetAxisRaw("Horizontal") * -1)); }    //Gravity Left     (270* degrees) [Negative X Axis]

        //********
        // GRAVITY
        //********

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

        //*********
        // CONTROLS
        //*********

        //Jump
        if (Input.GetKeyDown(KeyCode.W))
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
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}

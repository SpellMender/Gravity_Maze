using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float cameraSmoothing;
    private Quaternion playerRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation camera effect to follow player rotation
        playerRotation = Player.instance.transform.rotation;
        cameraSmoothing = GameController.instance.cameraSmoothing;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, cameraSmoothing * Time.deltaTime);

        //Keep view centered on the player
        transform.position = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y, transform.position.z);
    }
}

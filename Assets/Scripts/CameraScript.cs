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
        playerRotation = Player.instance.transform.rotation;
        cameraSmoothing = GameController.instance.cameraSmoothing;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, cameraSmoothing * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    public Camera mainCam;
    public Vector3 cameraMovement;
    public Vector3 playerMovement;
    public Vector3 targetPosition;
    public PlayerMovement playerMove;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = mainCam.transform.position;
        playerMove = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            targetPosition = mainCam.transform.position += cameraMovement;
            other.transform.position += playerMovement;
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, targetPosition, Time.deltaTime * 1.0f);
        }
    }
}

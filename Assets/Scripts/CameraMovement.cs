using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    public Camera mainCam;
    public Vector3 cameraMovement;
    public Vector3 playerMovement;
    public Vector3 startPos;
    public Vector3 targetPos;
    public GameObject player;
    public PlayerMovement playerMove;
    public float moveSpeed = 2.0f;
    private bool startMoving = false;
    

    // Start is called before the first frame update
    void Start()
    {
        targetPos = mainCam.transform.position;
        player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startMoving)
        {
            
            Debug.Log("mainCamPos Before: " + mainCam.transform.position);
            playerMove.CanMove(false);
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, targetPos, moveSpeed * 50 * Time.deltaTime);        
            Debug.Log("mainCamPos After: " + mainCam.transform.position);
            if (mainCam.transform.position == targetPos)
            {
                playerMove.CanMove(true);
                startMoving = false;
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            startPos = mainCam.transform.position;
            Debug.Log("TargetPos Before: " + targetPos);
            targetPos = mainCam.transform.position + cameraMovement;
            Debug.Log("TargetPos After: " + targetPos);
            other.transform.position += playerMovement;
            startMoving = true;
            
        }
    }
}

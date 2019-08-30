using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockDirections
{
    UP_DOWN,
    LEFT_RIGHT,
    BOTH
}

public enum TargetAction
{
    SHOW,
    HIDE
}
public class BlockPush : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private bool wasMoved = false; //After block is moved, prevents moving again
    private bool isInteractable = false; //flag to check the player is interacting
    private bool startMoving = false; //flag to begin moving the block
    private float pushTime; //used to check how long the user has been pushing
    private Vector3 startPos; //Where the block is located
    private Vector3 targetPos = Vector3.zero; //where the block will be moved too
    private float pushCount = 0.0f;

    public bool isMoveable = true;
    public BlockDirections moveDirection;
    public float waitTime = 0.75f;
    public float moveSpeed = 0.75f;
    public GameObject targetObject;
    public TargetAction visibility;
    public float pushLimit = 3.0f;


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.bodyType = RigidbodyType2D.Kinematic;

        startPos = transform.position;
        pushTime = waitTime;
    }

    private void Start()
    {
        if (visibility == TargetAction.SHOW)
        {
            targetObject.SetActive(false);
        }
        if (visibility == TargetAction.HIDE)
        {
            targetObject.SetActive(true);
        }
    }

    private void Update()
    {
       
        if (startMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
          

            if (transform.position.Equals(targetPos))
            {
                startMoving = false;
             
                if (pushCount == pushLimit)
                {
                    wasMoved = true;
                    isMoveable = false;
                }

                startPos = transform.position;

                if (visibility == TargetAction.SHOW)
                {
                    targetObject.SetActive(true);
                }
                if(visibility == TargetAction.HIDE)
                {
                    targetObject.SetActive(false);
                }
            }
        }
    }

    private void MoveBlock(Collision2D collision, BlockDirections moveDirection) {
        
        if ((moveDirection == BlockDirections.LEFT_RIGHT)||(moveDirection == BlockDirections.BOTH)) {
            
            if (collision.relativeVelocity.x < 0)
            {
                //Pushing left
                pushTime -= Time.deltaTime;

                if (pushTime < 0)
                {
                    pushCount++;
                    isInteractable = false;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    targetPos = new Vector2(startPos.x - 1, startPos.y);
                    startMoving = true;
                }

            }
            else if (collision.relativeVelocity.x > 0)
            {
                //Pushing right
                pushTime -= Time.deltaTime;

                if (pushTime < 0)
                {
                    pushCount++;
                    isInteractable = false;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    targetPos = new Vector2(startPos.x + 1, startPos.y);
                    startMoving = true;
                }

            }
        }
        if ((moveDirection == BlockDirections.UP_DOWN) || (moveDirection == BlockDirections.BOTH))
        {
            if (collision.relativeVelocity.y > 0)
            {
                //Pushing up
                pushTime -= Time.deltaTime;

                if (pushTime < 0)
                {
                    pushCount++;
                    isInteractable = false;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    targetPos = new Vector2(startPos.x, startPos.y + 1);
                    startMoving = true;
                }

            }
            else if (collision.relativeVelocity.y < 0)
            {
                //Pushing Down
                pushTime -= Time.deltaTime;

                if (pushTime < 0)
                {
                    pushCount++;
                    isInteractable = false;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    targetPos = new Vector2(startPos.x, startPos.y - 1);
                    startMoving = true;
                }

            }
        }
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isMoveable & !wasMoved && !startMoving)
        {
            isInteractable = true;
            pushTime = waitTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.relativeVelocity == Vector2.zero)
        {
            pushTime = waitTime;
        }

        //Checks if object is the player and if so, is the player interacting with block
        if (collision.gameObject.CompareTag("Player") && isInteractable)
        {

            switch (moveDirection)
            {
                case BlockDirections.LEFT_RIGHT:
                    //MoveLeftRight(collision);
                    MoveBlock(collision, moveDirection);
                    break;
                case BlockDirections.UP_DOWN:
                    //MoveUpDown(collision);
                    MoveBlock(collision, moveDirection);
                    break;
                case BlockDirections.BOTH:
                    //MoveLeftRight(collision);
                    MoveBlock(collision, moveDirection);
                    break;
            }

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isInteractable)
        {
            isInteractable = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    frozen,

    charging
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D playerRigidbody;
    private Vector3 change;
    private Animator anim;
    private bool canMove;

    private bool touchingObject;


    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        currentState = PlayerState.walk;
        touchingObject = false;
        anim.SetFloat("movex", 0);
        anim.SetFloat("movey", -1);

    }

    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");


            if (Input.GetButtonDown("attack"))
            {
                StartCoroutine(AttackCo());
            }
            else if ((currentState == PlayerState.walk) || (currentState == PlayerState.charging))
            {
                UpdateAnimationsAndMove();
            }


        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Spin Attack"))
        {
            while (anim.GetCurrentAnimatorStateInfo(0).IsName("Spin Attack"))
            {
                canMove = false;
            }
            canMove = true;
        }



    }

    void OnCollisionEnter2D(Collision2D col)
    {
        touchingObject = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        touchingObject = false;
    }

    public void CanMove(bool value)
    {
        canMove = value;
    }
    private IEnumerator AttackCo()
    {
        anim.SetBool("Attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        anim.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.30f);
        if (Input.GetButton("attack"))
        {
            currentState = PlayerState.charging;
            anim.SetBool("Charging", true);
        }
        else
        {
            currentState = PlayerState.walk;
        }

    }


    void UpdateAnimationsAndMove()
    {
        if (change != Vector3.zero)
        {
            MovePlayer();
            if (currentState != PlayerState.charging)
            {
                anim.SetFloat("movex", change.x);
                anim.SetFloat("movey", change.y);
            }
            anim.SetBool("moving", true);

            if (touchingObject)
            {
                anim.SetBool("Pushing", true);
            }
            if (!touchingObject)
            {
                anim.SetBool("Pushing", false);
            }
        }
        else
        {
            anim.SetBool("moving", false);
            anim.SetBool("Pushing", false);
        }

        if (currentState == PlayerState.charging)
        {
            if (!(Input.GetButton("attack")))
            {
                currentState = PlayerState.walk;
                anim.SetBool("Charging", false);
            }

        }
    }
    void MovePlayer()
    {
        change.Normalize();

        playerRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }


}

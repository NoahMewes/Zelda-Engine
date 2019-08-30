using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    walk,
    attack,
    interact
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;  
    private Rigidbody2D playerRigidbody;
    private Vector3 change;
    private Animator anim;
    private bool canMove;
   

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        currentState = PlayerState.walk;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) {
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");


            if (Input.GetButtonDown("attack"))
            {
                StartCoroutine(AttackCo());
            }
            else if (currentState == PlayerState.walk)
            {
                UpdateAnimationsAndMove();
            }
        }
        
        
    }
    public void CanMove(bool value) {
        canMove = value;
    }
    private IEnumerator AttackCo()
    {
        anim.SetBool("Attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        anim.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.30f);
        currentState = PlayerState.walk;
    }
    void UpdateAnimationsAndMove() {
        if (change != Vector3.zero)
        {
            MovePlayer();
            anim.SetFloat("movex", change.x);
            anim.SetFloat("movey", change.y);
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }

    }
    void MovePlayer() {
        change.Normalize();
        
        playerRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCamera;
    public GameObject player;
    public PlayerMovement playerMove;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.SetActive(true);

        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.SetActive(false);
            StartCoroutine(RoomTransition());
        }

    }

    private IEnumerator RoomTransition()
    {
        playerMove.CanMove(false);
        yield return new WaitForSeconds(0.4f);
        playerMove.CanMove(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItBehaviour : MonoBehaviour
{
    public float speed = 8f;
    public float trajectoryAngle = 0.75f;
    public string playerCharacter = "Fox";

    private bool isHeld;
    private Rigidbody2D rb;
    private Vector2 vector2;
    private GameObject player;
    // Collision detection
    void OnCollisionEnter2D(Collision2D col)
    {
        // if the object thats been collided with is the player
        // we want to trigger 'it" to be in a held state, and then
        // turn physics off for more performant updating
        if(col.gameObject.name == playerCharacter){
                isHeld = true;
                rb.isKinematic = true;
                player = col.gameObject;
        }
    }

    void FixedUpdate() {
        if(isHeld){
            // update the object to be above player
            rb.position = new Vector2(player.transform.position.x, player.transform.position.y + 1);
            // this is for "throwing" the object
            if(Input.GetKeyDown(KeyCode.Space)){
                isHeld = false;
                // throwing the object on an angle, depending on player input direction
                rb.velocity = new Vector2(Input.GetAxis("Horizontal") * (speed * trajectoryAngle), speed);
                // make sure to turn physics back on!
                rb.isKinematic = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isHeld = false;
        rb = GetComponent<Rigidbody2D>();
    }
}

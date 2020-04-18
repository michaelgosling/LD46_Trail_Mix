using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItBehaviour : MonoBehaviour
{
    public float speed = 8f;
    private bool isHeld;
    private Rigidbody2D rb;
    private Vector2 vector2;
    private GameObject fox;
    // Collision detection
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Fox"){
                isHeld = true;
                rb.isKinematic = true;
                fox = col.gameObject;
        }
    }

    void FixedUpdate() {
        if(isHeld){
            rb.position = new Vector2(fox.transform.position.x, fox.transform.position.y + 1);
            if(Input.GetKeyDown(KeyCode.Space)){
                isHeld = false;
                Vector2 throwVelocity = new Vector2(Input.GetAxis("Horizontal") * (speed/2), speed);
                rb.velocity = throwVelocity;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

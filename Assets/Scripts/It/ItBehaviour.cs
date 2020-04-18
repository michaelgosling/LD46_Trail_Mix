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
            var newRB = fox.transform.position;
            newRB.y += 1;
            rb.position = newRB;
            if(Input.GetKeyDown(KeyCode.Space)){
                isHeld = false;
                rb.velocity = Vector2.up * speed;
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

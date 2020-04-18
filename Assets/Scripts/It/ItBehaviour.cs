using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItBehaviour : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 vector2;
    // Collision detection
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log($"gameobject collision: {col.gameObject.name}");
        if(col.gameObject.name == "Fox"){
            rb.velocity = Vector2.up * speed;
            // Debug.Log("collision ok! updating!");
            // Vector2 v2 = new Vector2(0, 10);
            // vector2 = v2.normalized * speed;
            // rb.MovePosition(rb.position + vector2 * Time.fixedDeltaTime);
        }
    }

    void FixedUpdate() {
        //Debug.Log("Updating shroom: " + vector2);
        //rb.MovePosition(rb.position + vector2 * Time.fixedDeltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

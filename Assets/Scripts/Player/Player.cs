using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static readonly int DEFAULT_HP = 100;

    public bool CarryingIt { get; private set; } = true;
    public int HP { get; private set; } = DEFAULT_HP;

    void OnCollisionEnter2D(Collision2D col)
    {

    }

    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v2 = new Vector2(Input.GetAxis("Horizontal"), 0);
        moveVelocity = v2.normalized * speed;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}

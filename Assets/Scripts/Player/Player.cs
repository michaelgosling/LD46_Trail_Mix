using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Default HP for the player.
    /// </summary>
    public static readonly int DEFAULT_HP = 100;
    /// <summary>
    /// The default speed for the player
    /// </summary>
    public static readonly float DEFAULT_SPEED = 1f;

    /// <summary>
    /// How much to reduce speed by when carrying It.
    /// </summary>
    public static readonly float CARRYING_SPEED_REDUCTION = 0.5f;

    /// <summary>
    /// If the player is carrying It
    /// </summary>
    /// <value>boolean</value>
    public bool CarryingIt { get; private set; } = true;
    /// <summary>
    /// Player health points
    /// </summary>
    /// <value>Integer</value>
    public int HP { get; private set; } = DEFAULT_HP;

    /// <summary>
    /// Player speed
    /// </summary>
    /// <value>Float</value>
    public float movementSpeed = DEFAULT_SPEED;

    /// <summary>
    /// Physics body
    /// </summary>
    private Rigidbody2D body;
    /// <summary>
    /// Player velocity
    /// </summary>
    private Vector2 velocity;

    /// <summary>
    /// Private reference to It.
    /// </summary>
    /// <value>GameObject</value>

    private GameObject It { get; set; }

    /// <summary>
    /// Collision Handling
    /// </summary>
    /// <param name="col">2D Collision that occured</param>
    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "It":
                CatchIt(col.gameObject);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Catch the "It" object and assign it to a private property to keep a reference, just in case.
    /// </summary>
    /// <param name="it">Object to keep alive.</param>
    void CatchIt(GameObject it)
    {
        CarryingIt = true;
        It = it;
    }

    /// <summary>
    /// Throw the "It" object
    /// </summary>
    public void ThrowIt()
    {
        if (CarryingIt) CarryingIt = false;
    }

    /// <summary>
    /// Initialization
    /// </summary>
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Logic to apply at an unfixed interval update
    /// </summary>
    void Update()
    {
        // Vector2 v2 = new Vector2(Input.GetAxis("Horizontal"), 0);
        // velocity = v2.normalized * Speed;
    }

    /// <summary>
    /// Apply user input to the player object
    /// </summary>
    void ApplyInput()
    {
        float xForce = 0.0f;
        float yForce = 0.0f;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            xForce = movementSpeed;
            yForce = 0.0f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            xForce = movementSpeed * -1;
            yForce = 0.0f;
        }

        // TODO: Cap velocity/speed
        Vector2 force = new Vector2(xForce, yForce);
        body.AddForce(force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Handle Movement Logic
    /// </summary>
    void Move()
    {
        if (body != null)
        {
            ApplyInput();
        }
        else
        {
            Debug.LogWarning("RigidBody not attached to player " + gameObject.name);
        }
    }


    /// <summary>
    /// Update logic to run at a fixed interval
    /// </summary>
    void FixedUpdate()
    {
        Move();
    }
}

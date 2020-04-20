using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /// <summary>
    /// Player Default Values
    /// </summary>
    public class Defaults
    {
        // using key name strings for input mapping, see https://docs.unity3d.com/Manual/class-InputManager.html
        public static readonly string JUMP_KEY = "space";
        public static readonly string MOVE_LEFT_KEY = "left";
        public static readonly string MOVE_RIGHT_KEY = "right";
        public static readonly string THROW_KEY = "z";
        // public static readonly int HP = 100;
        public static readonly float X_SPEED = 10f;
        public static readonly float CARRYING_VELOCITY_FACTOR = 0.66f;
    }

    // Input Mapping
    /// <summary>
    /// Jump Key by Name, See https://docs.unity3d.com/Manual/class-InputManager.html
    /// </summary>
    public string jumpKey = Player.Defaults.JUMP_KEY;

    /// <summary>
    /// Move Left key by name, See https://docs.unity3d.com/Manual/class-InputManager.html
    /// </summary>
    public string moveLeftKey = Player.Defaults.MOVE_LEFT_KEY;

    /// <summary>
    /// Move Right Key by name, See https://docs.unity3d.com/Manual/class-InputManager.html
    /// </summary>
    public string moveRightKey = Player.Defaults.MOVE_RIGHT_KEY;

    /// <summary>
    /// Throw Key by name, See https://docs.unity3d.com/Manual/class-InputManager.html
    /// </summary>
    public string throwKey = Player.Defaults.THROW_KEY;

    /// <summary>
    /// Factor to apply to x Velocity when carrying It.
    /// </summary>
    public float carryingVelocityFactor = Player.Defaults.CARRYING_VELOCITY_FACTOR;


    /// <summary>
    /// If the player is carrying It
    /// </summary>
    /// <value>boolean</value>
    public bool CarryingIt { get; private set; } = true;

    // public int HP { get; private set; } = Player.Defaults.HP;

    /// <summary>
    /// Player speed
    /// </summary>
    /// <value>Float</value>
    public float xSpeed = Player.Defaults.X_SPEED;

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
        if (It == null) It = it;
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
    /// Update the player's velocity
    /// </summary>
    void UpdateVelocity()
    {
        float newXVelocity = 0.0f;

        if (Input.GetKey(moveRightKey))
        {
            newXVelocity = xSpeed;
        }
        else if (Input.GetKey(moveLeftKey))
        {
            newXVelocity = xSpeed * -1;
        }


        body.velocity = velocity = new Vector2(newXVelocity * (CarryingIt ? carryingVelocityFactor : 1), body.velocity.y);
    }


    /// <summary>
    /// Handle Movement Logic
    /// </summary>
    void UpdateMovement()
    {
        if (body != null)
        {
            UpdateVelocity();
        }
        else
        {
            Debug.LogWarning("RigidBody not attached to player " + gameObject.name);
        }
    }

    void UpdateState()
    {
        if (CarryingIt && Input.GetKeyDown(throwKey))
        {
            ThrowIt();
        }
    }


    /// <summary>
    /// Update logic to run at a fixed interval
    /// </summary>
    void FixedUpdate()
    {
        if (Debug.isDebugBuild && Input.anyKeyDown)
        {
            var logMsg = "Inputs Activated: ";
            if (Input.GetKeyDown(jumpKey)) logMsg += "[Jump]";
            if (Input.GetKeyDown(moveLeftKey)) logMsg += "[Move Left]";
            if (Input.GetKeyDown(moveRightKey)) logMsg += "[Move Right]";
            if (Input.GetKeyDown(throwKey)) logMsg += "[Throw]";
            //Debug.Log(logMsg);
        }
        UpdateState();
        UpdateMovement();
    }
}

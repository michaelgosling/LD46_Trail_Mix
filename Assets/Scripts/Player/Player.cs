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
        // public static readonly string DASH_KEY = "x";
        public static readonly float X_SPEED = 10f;
        public static readonly float Y_SPEED = 10f;
        // public static readonly float DASH_SPEED_MOD = 2f;
        public static readonly float CARRYING_VELOCITY_FACTOR = 0.66f;
    }

    #region Input Mapping

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
    /// Dash Key by name, See https://docs.unity3d.com/Manual/class-InputManager.html
    /// </summary>
    // public string dashKey = Player.Defaults.DASH_KEY;

    #endregion

    #region State

    bool moveRightPressed;
    bool moveLeftPressed;
    bool jumpPressed;
    bool jumpExhausted;
    // bool dashPressed;
    // bool dashExhausted;
    bool facingRight;
    // bool dashing;
    bool jumping;

    #endregion

    #region Fields

    /// <summary>
    /// Factor to apply to x Velocity when carrying It.
    /// </summary>
    public float carryingVelocityFactor = Player.Defaults.CARRYING_VELOCITY_FACTOR;

    /// <summary>
    /// Player x speed
    /// </summary>
    public float xSpeed = Player.Defaults.X_SPEED;

    /// <summary>
    /// Player y speed
    /// </summary>
    public float ySpeed = Player.Defaults.Y_SPEED;

    /// <summary>
    /// Modifier to apply when dashing
    /// </summary>
    // public float dashSpeedMod = Player.Defaults.DASH_SPEED_MOD;

    /// <summary>
    /// Physics body
    /// </summary>
    private Rigidbody2D body;

    /// <summary>
    /// Player Animator Controller
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Player Sprite Renderer
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Player velocity
    /// </summary>
    private Vector2 velocity;

    #endregion

    /// <summary>
    /// Collision Handling
    /// </summary>
    /// <param name="col">2D Collision that occured</param>
    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Floor":
                jumpExhausted = false;
                jumping = false;
                // dashExhausted = false;
                // dashing = false;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Initialization
    /// </summary>
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        velocity = body.velocity;

        moveLeftPressed = false;
        moveRightPressed = false;
        jumpPressed = false;
        // dashPressed = false;

        jumpExhausted = false;
        // dashExhausted = false;

        // dashing = false;
        jumping = false;

        facingRight = true;
    }


    /// <summary>
    /// Parse Player Input and store in booleans to reference later when applying state.
    /// </summary>
    void ParseInput()
    {
        moveRightPressed = Input.GetKey(moveRightKey);
        moveLeftPressed = Input.GetKey(moveLeftKey);
        jumpPressed = Input.GetKeyDown(jumpKey);
    }

    /// <summary>
    /// Update the player's velocity
    /// </summary>
    void UpdateVelocity()
    {
        var velocityMod = Global.Instance.it.Held ? carryingVelocityFactor : 1;

        float newXVelocity = 0.0f;
        float newYVelocity = body?.velocity.y ?? 0.0f;

        if (moveRightPressed)
        {
            newXVelocity += xSpeed;
            if (!moveLeftPressed) facingRight = true;
        }
        if (moveLeftPressed)
        {
            newXVelocity += xSpeed * -1;
            if (!moveRightPressed) facingRight = false;
        }
        if (jumpPressed && !jumpExhausted)
        {
            newYVelocity += ySpeed;
            jumpExhausted = true;
            jumping = true;
        }

        velocity = new Vector2(newXVelocity * velocityMod, newYVelocity * velocityMod);
    }

    /// <summary>
    /// Update player state
    /// </summary>
    void UpdateState()
    {
        ParseInput();
        UpdateVelocity();
    }

    /// <summary>
    /// Apply the updated state to the game object
    /// </summary>
    void ApplyState()
    {
        // apply velocity to rigidbody
        body.velocity = velocity;

        // update animation parameters and sprite renderer
        spriteRenderer.flipX = !facingRight;
        animator.SetBool("holdingBun", Global.Instance.it.Held);
        animator.SetBool("jumping", jumping);
        animator.SetFloat("Speed", Mathf.Abs(velocity.x));

        if (Debug.isDebugBuild)
        {
            var logMsg = "holdingBun: " + animator.GetBool("holdingBun");
            logMsg += ", jumping: " + animator.GetBool("jumping");
            logMsg += ", Speed: " + animator.GetFloat("Speed");
            Debug.Log(logMsg);
        }

    }


    /// <summary>
    /// Update logic to run at a fixed interval
    /// </summary>
    void FixedUpdate()
    {
        UpdateState();
        ApplyState();
    }
}

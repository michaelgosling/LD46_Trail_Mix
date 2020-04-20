using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItBehaviour : MonoBehaviour
{
    public class Defaults 
    {
        public static readonly float SPEED = 8.0f;
        public static readonly float TRAJECTORY_ANGLE = 0.75f;
        public static readonly float CARRY_HEIGHT = 1.7f;
        public static readonly string PLAYER_CHARACTER = "Fox";
        public static readonly string THROW_KEY = "z";
    }

    public string playerTag = "Player";
    public Sprite freeFallSprite;
    public Sprite throwSprite;
    public float speed = ItBehaviour.Defaults.SPEED;
    public float trajectoryAngle = ItBehaviour.Defaults.TRAJECTORY_ANGLE;
    public float carryHeight = ItBehaviour.Defaults.CARRY_HEIGHT;
    public string playerCharacter = ItBehaviour.Defaults.PLAYER_CHARACTER;
    public string throwKey = ItBehaviour.Defaults.THROW_KEY;

    public bool Held { get { return isHeld; } }

    private bool isHeld;
    private bool isActive;
    private bool isDead;
    private Rigidbody2D rb;
    private BoxCollider2D itCollider;
    private Vector2 vector2;
    private GameObject player;
    private SpriteRenderer itSprite;
    // Collision detection
    void OnCollisionEnter2D(Collision2D col)
    {
        // if the object thats been collided with is the player
        // we want to trigger 'it" to be in a held state, and then
        // turn physics off for more performant updating
        if (col.gameObject.tag == playerTag)
        {
            isHeld = isActive = rb.isKinematic = true;
            itCollider.enabled = false;
            player = col.gameObject;
        }
        else if (isActive)
        {
            isActive = isHeld = false;
            rb.position = new Vector2(Camera.main.transform.position.x, 8);
            Global.Instance.LifeLost(this.rb);
        }
    }

    void FixedUpdate()
    {
        if (isHeld)
        {
            itSprite.enabled = false;
            // update the object to be above player
            rb.position = new Vector2(player.transform.position.x, player.transform.position.y + carryHeight);
            // this is for "throwing" the object
            if(Input.GetKeyDown(throwKey)){
                // throwing the object on an angle, depending on player input direction
                rb.velocity = new Vector2(Input.GetAxis("Horizontal") * (speed * trajectoryAngle), speed);
                // make sure to turn physics back on!
                rb.isKinematic = false;
                isHeld = false;
            }
        }
        else 
        {
            itSprite.enabled = true;
            // get direction of bun travel, if going down, open the chute.
            itCollider.enabled = true;
            var travel = transform.InverseTransformDirection(rb.velocity);
            Debug.Log(travel.y);
            if(travel.y < -1) 
            { 
                itSprite.sprite = freeFallSprite;
                // no fall off map, no need buggy code
                // if(travel.y < -10){
                //     rb.position = new Vector2(Camera.main.transform.position.x, 8);
                //     if(!isActive) 
                //     {
                //         isActive = isHeld = false;
                //         Global.Instance.LifeLost(rb);
                //     }
                // }
            }
            else
                itSprite.sprite = throwSprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isHeld = false;
        rb = GetComponent<Rigidbody2D>();
        itCollider = GetComponent<BoxCollider2D>();
        itSprite = GetComponent<SpriteRenderer>();
    }
}

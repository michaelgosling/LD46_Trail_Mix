using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItBehaviour : MonoBehaviour
{
    public class Defaults 
    {
        public static readonly float SPEED = 8.0f;
        public static readonly float TRAJECTORY_ANGLE = 0.75f;
        public static readonly float CARRY_HEIGHT = 0.5f;
        public static readonly string PLAYER_CHARACTER = "Fox";
        public static readonly string THROW_KEY = "z";
    }

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
    private Rigidbody2D rb;
    private Vector2 vector2;
    private GameObject player;
    private SpriteRenderer itSprite;
    // Collision detection
    void OnCollisionEnter2D(Collision2D col)
    {
        // if the object thats been collided with is the player
        // we want to trigger 'it" to be in a held state, and then
        // turn physics off for more performant updating
        if (col.gameObject.name == playerCharacter)
        {
            isHeld = isActive = rb.isKinematic = true;
            player = col.gameObject;
        }
        else if (isActive)
        {
            isActive = isHeld = false;
            Global.Instance.LifeLost(this.rb);
            // StartCoroutine(GroundHit(itSprite));
            // Tilemap Floor <-- we may want to add specific collision?
            // for now, it's anything that isn't your player after initial pick up.
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
                isHeld = false;
                // throwing the object on an angle, depending on player input direction
                rb.velocity = new Vector2(Input.GetAxis("Horizontal") * (speed * trajectoryAngle), speed);
                // make sure to turn physics back on!
                rb.isKinematic = false;
            }
        }
        else 
        {
            itSprite.enabled = true;
            gameObject.SetActive(true);
            if(rb.position.y < Camera.main.transform.position.y - 10){
                Global.Instance.LifeLost(rb);
            }
            //get direction of bun travel, if going down, open the chute.
            var travel = transform.InverseTransformDirection(rb.velocity);
            if(travel.y < -1)
                itSprite.sprite = freeFallSprite;
            else
                itSprite.sprite = throwSprite;
            Debug.Log($"Bun go: {travel.y}");
        }
    }

    private IEnumerator GroundHit(SpriteRenderer sprite)
    {
        // this makes it drop throught the map.
        // GetComponent<BoxCollider2D>().enabled = false;

        // animation of color change (could be whatever) to indicate that it ded.
        WaitForSeconds wait = new WaitForSeconds(0.4f);
        for (var i = 0; i < 5; i++)
        {
            sprite.color = i % 2 == 0 ? Color.cyan : Color.red;
            yield return wait;
        }
        // peace out gangsta
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        isHeld = false;
        rb = GetComponent<Rigidbody2D>();
        itSprite = GetComponent<SpriteRenderer>();
    }
}

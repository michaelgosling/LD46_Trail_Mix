using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private CircleCollider2D hitbox;
    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<CircleCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player") && Global.Instance.it.Held) Global.Instance.CompleteLevel();
    }
}

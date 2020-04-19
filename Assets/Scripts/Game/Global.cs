using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    // the global we share
    public static Global Instance;
    // global persistant vars we want
    public int lives = 3;

    
    // https://giphy.com/gifs/star-trek-tng-the-next-generation-bKnEnd65zqxfq/tile
    void Awake() 
    {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void LifeLost(Rigidbody2D rb)
    {   
        this.lives--;
        if(this.lives == 0){
            Debug.Log("GAME OVER LOOOOOOSER");
        } else {
            rb.position = new Vector2(1,2);
        }
    }
}

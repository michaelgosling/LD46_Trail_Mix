using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    private string lifeBlood = "♥";
    public Text lifeText;

    // Update is called once per frame
    void Update()
    {
        var lifeDisplay = "";
        for(var i = 0; i < Global.Instance.lives; i++){
            lifeDisplay += lifeBlood;
        }
        lifeText.text = "Lives: " + lifeDisplay;
    }
}

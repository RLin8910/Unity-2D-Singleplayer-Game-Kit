using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Game goal. Ends the game when the trigger is reached. </summary>
public class Goal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) 
    {
        // end the game and use the gameObject's name as the winner name
        if(collider.isTrigger) return;
        GameManager.instance.EndGame(true);
    }
}

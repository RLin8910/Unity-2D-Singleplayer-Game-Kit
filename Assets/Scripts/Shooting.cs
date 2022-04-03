using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Player shooting using mouse input. </summary>
public class Shooting : MonoBehaviour
{
    ///<summary> The bullet prefab. </summary>
    [SerializeField]
    private GameObject bullet;
    ///<summary> The button axis used to shoot. </summary>
    [SerializeField]
    private string shootButton;
    ///<summary> The rigidbody attached to the player. </summary>
    [SerializeField]
    private Rigidbody2D rigidbody;

    void Update()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = target - (Vector2)transform.position;
        if(Input.GetButtonDown(shootButton))
        {
            Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>().parent = rigidbody;
        }
    }
}

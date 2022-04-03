using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Controls bullets. </summary>
public class Bullet : MonoBehaviour
{
    [HideInInspector]
    /// <summary> The object that spawned this bullet - will be ignored when checking collisions. </summary>
    public Rigidbody2D parent;
    /// <summary> The rigidbody for this bullet. </summary>
    [SerializeField]
    private Rigidbody2D rigidbody;
    /// <summary> How fast this bullet moves. </summary>
    [SerializeField]
    private float speed = 20;
    /// <summary> The amount of time to keep a bullet loaded before it despawns. </summary>
    [SerializeField]
    private float maxAliveTime = 5;
    /// <summary> The amount of damage this bullet does. </summary>
    [SerializeField]
    private float damage = 0.1f;

    IEnumerator Start()
    {
        rigidbody.velocity = transform.up * speed;
        yield return new WaitForSeconds(maxAliveTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.attachedRigidbody == parent) return;
        // cause other object to take damage if necessary
        other.SendMessageUpwards("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}

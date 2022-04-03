using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Basic player class. Includes configurable controls and gravity. </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    // constants
    /// <summary> Constant used for determining if the player is grounded or not </summary>
    private const float VERTICAL_THRESHOLD = 0.7f;
    /// <summary> Constant to give some leeway with whether raycasts detect if the player is grounded </summary>
    private const float RAYCAST_MULTIPLIER = 1.05f;
    // static variables
    /// <summary> Toggles whether players can move. </summary>
    public static bool CanMove {get; set;} = false;
    /// <summary> List of all players in the game. </summary>
    private static List<Player> Players = new List<Player>();
    // inspector variables
    [Header("Input")]
    /// <summary> The input axis used to control horizontal movement. Leave blank to disable. </summary>
    [SerializeField]
    private string horizontal;
    /// <summary> The input axis used to control vertical movement, excluding jumps. Leave blank to disable. 
    /// Also disabled if rigidbody2D component uses gravity.
    /// </summary>
    [SerializeField]
    private string vertical;
    /// <summary> The input axis used for jumps. Leave blank to disable. </summary>
    [SerializeField]
    private string jump;
    [Header("Speed")]
    /// <summary> How fast the player moves in the horizontal/vertical directions, excluding jumps. </summary>
    [SerializeField]
    private float speed;
    /// <summary> Controls how high the player jumps. </summary>
    [SerializeField]
    private float jumpVelocity;
    [Header("Health")]
    /// <summary> The max health of the player. </summary>
    [SerializeField]
    private float maxHealth = 1;
    /// <summary> The player's color at full health. </summary>
    [SerializeField]
    private Color normalColor;
    /// <summary> The player's color at minimum health. </summary>
    [SerializeField]
    private Color deadColor;
    // private variables, not editable in inspector
    /// <summary> The player's rigidbody, used for physics-based movement. </summary>
    #pragma warning disable CS0108 // disable an old Unity warning that should have been deprecated years ago
    private Rigidbody2D rigidbody2D;
    /// <summary> Used for determining if the player is grounded. </summary>
    private BoxCollider2D boxCollider2D;
    /// <summary> The visuals for this player. </summary>
    private SpriteRenderer[] spriteRenderers;
    /// <summary> The player's start location. </summary>
    private Vector2 startPosition;
    /// <summary> The player's start rotation. </summary>
    private Quaternion startRotation;
    /// <summary> The player's current health. </summary>
    private float health;
    // private functions
    // Initialization code
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        startPosition = transform.position;
        startRotation = transform.rotation;

        ResetPlayer();

        Players.Add(this);
    }

    // Do movement in FixedUpdate since it's based on Unity's Physics
    void FixedUpdate() 
    {
        Vector2 velocity = rigidbody2D.velocity;
        Vector2 input = Vector2.zero;
        if(!string.IsNullOrEmpty(horizontal) && CanMove) input.x = Input.GetAxis(horizontal);
        if(!string.IsNullOrEmpty(vertical) && CanMove) input.y = Input.GetAxis(vertical);
        if(input.sqrMagnitude > 1) input.Normalize(); // prevent player from moving faster along diagonals

        // ignore y input if we use gravity
        if(rigidbody2D.gravityScale > 0)
        {
            velocity.x = input.x * speed;
            // jump input
            if(!string.IsNullOrEmpty(jump) && Input.GetButton(jump) && CanMove){
                // check if player is grounded
                // raycast on left of player and right of player
                RaycastHit2D left = Physics2D.Raycast(
                    new Vector2(boxCollider2D.bounds.center.x - boxCollider2D.bounds.extents.x / RAYCAST_MULTIPLIER, boxCollider2D.bounds.center.y),
                    Vector2.down,
                    boxCollider2D.bounds.extents.y * RAYCAST_MULTIPLIER
                );
                RaycastHit2D right = Physics2D.Raycast(
                    new Vector2(boxCollider2D.bounds.center.x + boxCollider2D.bounds.extents.x / RAYCAST_MULTIPLIER, boxCollider2D.bounds.center.y),
                    Vector2.down,
                    boxCollider2D.bounds.extents.y * RAYCAST_MULTIPLIER
                );
                if((left.collider != null && Vector2.Dot(left.normal, Vector2.up) > VERTICAL_THRESHOLD) ||
                   (right.collider != null && Vector2.Dot(right.normal, Vector2.up) > VERTICAL_THRESHOLD))
                {
                    // jump
                    velocity.y = jumpVelocity;
                }
            }
        }
        else
        {
            velocity = input * speed;
        }

        rigidbody2D.velocity = velocity;
    }
    // make sure player is removed from list when destroyed
    void OnDestroy()
    {
        Players.Remove(this);
    }
    /// <summary> Colors the player according to their current health. </summary>
    private void ColorPlayer()
    {
        float h1, s1, v1, h2, s2, v2;
        Color.RGBToHSV(normalColor, out h1, out s1, out v1);
        Color.RGBToHSV(deadColor, out h2, out s2, out v2);
        
        Color color = Color.HSVToRGB(
            Mathf.Lerp(h2, h1, health / maxHealth),
            Mathf.Lerp(s2, s1, health / maxHealth),
            Mathf.Lerp(v2, v1, health / maxHealth)
        );

        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = color;
        }
    }   
    // public functions
    /// <summary> Reset this player. </summary>
    public void ResetPlayer()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        health = maxHealth;

        ColorPlayer();
    }
    /// <summary> Take the specified amount of damage. </summary>
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0) health = 0;

        ColorPlayer();
    }
    // static functions
    /// <summary> Reset the positions of all players. </summary>
    public static void ResetAllPositions ()
    {
        foreach(Player player in Players)
        {
            player.ResetPlayer();
        }
    }
}

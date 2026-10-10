using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Minimum speed of the enemy")]
    private float speedRangeMin = 2f;
    [SerializeField]
    [Tooltip("Maximum speed of the enemy")]
    private float speedRangeMax = 4f;

    [SerializeField]
    [Tooltip("Distance from the player at which the enemy will destroy itself")]
    private float playerDestroyDistance = 0.2f;

    [SerializeField]
    [Tooltip("Force applied to the enemy when pushed away by the player on meter full")]
    private float pushedAwayForce = 5f;

    // The actual speed of the enemy, randomly set between speedRangeMin and speedRangeMax
    private float speed;

    [SerializeField]
    [Tooltip("Check if good. Uncheck if bad")]
    private bool isGood = false;

    private Transform player;

    [SerializeField]
    [Tooltip("Sprite list models good or bad")]
    private List<Sprite> spriteList;

    private SpriteRenderer sr;
    private Animator animator;

    private void OnEnable()
    {
        GameManager.GameStateHandler += OnGameStateChanged;
        PlayerController.OnMeterFull += OnMeterFull;
    }

    /// <summary>
    /// This method is called when the player fills their meter. (EVENT)
    /// It pushes away the enemy if it is a "bad girl" (hostile). 
    /// If the enemy is a "good girl" (friendly), it does nothing.
    /// </summary>
    private void OnMeterFull()
    {
        if (isGood) return;
        Vector3 direction = (player.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(-direction * pushedAwayForce, ForceMode2D.Impulse);
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        speed = Random.Range(speedRangeMin, speedRangeMax);

        // Randomly determine if this enemy is a "good girl" (friendly) or not
        int randomInt = Random.Range(0, 3);

        Debug.Log("Random int: " + randomInt);
        sr.sprite = spriteList[randomInt];

        // MAKESURE: Make sure that the sprite list has the same index than the Type integer value in the animator
        animator.SetFloat("Blend", randomInt);
    }

    // Update is called once per frame
    void Update()
    {
        // If close to player destroy itself
        if (player != null && Vector3.Distance(transform.position, player.position) < playerDestroyDistance)
        {
            var pc = player.GetComponent<PlayerController>();
            if (isGood)
            {
                pc.AddPoint(1);
            }else
            {
                pc.GetComponent<PlayerStats>().TakeDamage(1);
                pc.RemovePoint(1);
            }

            Destroy(gameObject);
        }

        // Move towards the player
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            // flip x according to the direction the enemy is going
            sr.flipX = direction.x < 0;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    // Requires rigidbody2D and collider2D on the enemy prefab for this to work
    // When the enemy is clicked, it will either add or remove points from the player based on whether it's a "good girl" or not
    private void OnMouseDown()
    {
        var pc = player.GetComponent<PlayerController>();
        if (isGood)
        {
            pc.RemovePoint(1);
        }
        else
        {
            pc.AddPoint(1);
        }

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        GameManager.GameStateHandler -= OnGameStateChanged;
        PlayerController.OnMeterFull -= OnMeterFull;
    }
}

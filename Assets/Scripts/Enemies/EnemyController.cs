using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    private const float TIME_BEFORE_DESTROYED_AFTER_PUSHED = 3f;
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

    [Tooltip("Check if good. Uncheck if bad")]
    public bool isGood = false;

    [HideInInspector]
    public Transform Player;

    [SerializeField]
    [Tooltip("Sprite list models good or bad")]
    private List<Sprite> spriteList;

    private SpriteRenderer sr;

    [SerializeField]
    private SpriteRenderer outlineSR;
    private Animator animator;

    private bool wasClicked = false;
    private float directionMultiplier = 1f;

    private AudioManager audioManager;

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
        Vector3 direction = (Player.position - transform.position).normalized;
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
        audioManager = AudioManager.Instance;

        Player = GameObject.FindGameObjectWithTag("Player").transform;
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
        if(wasClicked)
        {
            directionMultiplier = -3f;
        }

        // If close to player destroy itself
        if (Player != null && Vector3.Distance(transform.position, Player.position) < playerDestroyDistance)
        {
            var pc = Player.GetComponent<PlayerController>();
            if (isGood)
            {
                pc.AddPoint(2);
                audioManager.PlayRandomLoverClip(0.15f);
            }
            else
            {
                pc.GetComponent<PlayerStats>().TakeDamage(1);
                pc.RemovePoint(2);
                audioManager.PlayRandomEnemyClip(0.3f);
            }

            Destroy(gameObject);
        }

        // Move towards the player
        if (Player != null)
        {
            Vector3 direction = (Player.position - transform.position).normalized;

            // flip x according to the direction the enemy is going
            sr.flipX = !isGood ? direction.x < 0 : direction.x > 0;
            outlineSR.flipX = sr.flipX;
            transform.position += direction * speed * directionMultiplier * Time.deltaTime;
        }
    }

    // Requires rigidbody2D and collider2D on the enemy prefab for this to work
    // When the enemy is clicked, it will either add or remove points from the player based on whether it's a "good girl" or not
    private void OnMouseDown()
    {
        var pc = Player.GetComponent<PlayerController>();
        if (isGood)
        {
            pc.RemovePoint(2);
        }
        else
        {
            pc.AddPoint(2);
        }

        wasClicked = true;
        Destroy(gameObject, TIME_BEFORE_DESTROYED_AFTER_PUSHED);
    }

    private void OnDisable()
    {
        GameManager.GameStateHandler -= OnGameStateChanged;
        PlayerController.OnMeterFull -= OnMeterFull;
    }
}

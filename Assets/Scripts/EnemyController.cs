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
    [Tooltip("Percentage of enemies that are 'bad girls' (hostile)")]
    private float percentageOfBadGirls = 75;

    // The actual speed of the enemy, randomly set between speedRangeMin and speedRangeMax
    private float speed;
    private bool isGood = false;

    private Transform player;

    private void OnEnable()
    {
        GameManager.GameStateHandler += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        speed = Random.Range(speedRangeMin, speedRangeMax);

        // Randomly determine if this enemy is a "good girl" (friendly) or not
        isGood = Random.value > 0.75f; // 25% chance to be a good girl
        GetComponent<SpriteRenderer>().color = isGood ? Color.white : Color.red;
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
                pc.RemovePoint(1);
            }

            Destroy(gameObject);
        }

        // Move towards the player
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
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
    }
}

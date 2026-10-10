using UnityEngine;

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

    // The actual speed of the enemy, randomly set between speedRangeMin and speedRangeMax
    private float speed;

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
    }

    // Update is called once per frame
    void Update()
    {
        // If close to player destroy itself
        if (player != null && Vector3.Distance(transform.position, player.position) < playerDestroyDistance)
        {
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
    private void OnMouseDown()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        GameManager.GameStateHandler -= OnGameStateChanged;
    }
}

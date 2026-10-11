using UnityEngine;

public class HandRotation : MonoBehaviour
{
    private Transform player;

    [Header("Orbit Range")]
    [SerializeField] private float minOrbitDistance = 0.8f; // Closest the gun can get to the player
    [SerializeField] private float maxOrbitDistance = 2.5f; // Furthest the gun can go


    private Animator animator;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Shoot");
        }

        if (player == null) return;

        // 1. Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2. Get the full distance vector from the Player to the Mouse
        Vector2 targetOffset = (Vector2)mousePosition - (Vector2)player.position;

        // 3. Extract the current raw distance (magnitude)
        float currentDistance = targetOffset.magnitude;

        // 4. Clamp that distance strictly between your min and max limits
        float clampedDistance = Mathf.Clamp(currentDistance, minOrbitDistance, maxOrbitDistance);

        // 5. Re-apply the clamped distance to the direction vector
        // (We normalize targetOffset safely to keep the clean angle direction)
        Vector2 finalOffset = targetOffset.normalized * clampedDistance;

        // 6. Move the gun to the clamped relative spot around the player
        transform.position = (Vector2)player.position + finalOffset;

        // 7. Calculate angle using the direction vector so rotation stays accurate
        float angle = Mathf.Atan2(targetOffset.y, targetOffset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}

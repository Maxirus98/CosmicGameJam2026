using UnityEngine;

public class HandsOrbiter : MonoBehaviour
{
    private Transform player;

    [SerializeField]
    private float handSpeed = 50f;

    [SerializeField]
    private GameObject handPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the hand around the player and towards the mouse like a gun around a 2d topdown player

    }

    private void RotateAroundPlayer()
    {
        
    }

    // Rotates the player towards the mouse
    private void RotateTowardsMouse()
    {
        // Rotate the player based on mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rot = Quaternion.Euler(0f, 0f, angle - 90f); // Subtract 90 degrees to align the sprite correctly
        transform.rotation = rot;
    }
}

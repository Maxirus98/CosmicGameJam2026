using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private Slider scoreSlider;
    [SerializeField]
    private int maxScore = 50;

    private int score = 0;

    public void AddPoint(int point)
    {
        if (this.score >= maxScore) return;

        this.score += point;
        scoreSlider.value = this.score;
    }

    public void RemovePoint(int point)
    {
        if (this.score <= 0) return;

        this.score -= point;
        scoreSlider.value = this.score;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the player based on mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Set z to 0 to match the player's z position
        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // Subtract 90 degrees to align the sprite correctly

        // Move the player based on WASD input
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime, Space.World);

        // Clamp the player's position to the camera's view
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -camWidth + 0.5f, camWidth - 0.5f);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -camHeight + 0.5f, camHeight - 0.5f);
        transform.position = clampedPosition;
    }
}

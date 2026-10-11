using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private Slider scoreSlider;
    [SerializeField]
    private int maxMeterValue = 50;

    [SerializeField]
    [Tooltip("Prefab of the enemy spawner to instantiate when the meter is full")]
    private GameObject enemySpawnerPrefab;

    private int chadMeterValue = 0;

    /// <summary>
    /// Event that is triggered when the meter is full.
    /// </summary>
    public static event Action OnMeterFull;
    private bool meterFullTriggered = false;

    // Dashing variables
    [SerializeField]
    [Tooltip("Speed at which the player dashes")]
    private float dashSpeed = 25f;
    private bool isDashing = false;

    [SerializeField]
    [Tooltip("Duration of the dash in seconds")]
    private float dashDuration = 0.5f;
    private float dashTimer = 0f;

    [SerializeField]
    [Tooltip("Dash cooldown in seconds")]
    private float dashCooldown = 1f;
    private float dashCooldownTimer = 0f;

    private Vector3 currentDirection;

    public void AddPoint(int point)
    {
        if (this.chadMeterValue >= maxMeterValue) return;

        this.chadMeterValue += point;
        scoreSlider.value = this.chadMeterValue;
        Debug.Log($"Adding {point} points to the meter. Current value: {this.chadMeterValue}");
    }

    public void RemovePoint(int point)
    {
        if (this.chadMeterValue <= 0) return;

        this.chadMeterValue -= point;
        scoreSlider.value = this.chadMeterValue;
    }

    private void Start()
    {
        scoreSlider.maxValue = maxMeterValue;
        chadMeterValue = maxMeterValue / 2;
        scoreSlider.value = chadMeterValue;
    }

    // Update is called once per frame
    void Update()
    {
        // RotateTowardsMouse();
        if(dashCooldownTimer < dashCooldown)
        {
            dashCooldownTimer += Time.deltaTime;
        }

        // Dash towards the mouse position when spacebar is clicked for 0.2 seconds
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCooldownTimer >= dashCooldown)
            {
                isDashing = true;
                dashCooldownTimer = 0f;
            }
        }

        if(isDashing)
        {
            dashTimer += Time.deltaTime;
            PerformDash(currentDirection);
            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                dashTimer = 0f;
            }
        }

        if (!isDashing)
        {
            // Move the player based on WASD input
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            currentDirection = new Vector3(moveX, moveY, 0f).normalized;
            transform.Translate(currentDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        // Clamp the player's position to the camera's view
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -camWidth + 0.5f, camWidth - 0.5f);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -camHeight + 0.5f, camHeight - 0.5f);
        transform.position = clampedPosition;

        // Check if the meter is full for the required time to access the next level
        if(chadMeterValue >= maxMeterValue)
        {
            OnMeterFull?.Invoke();
            GetComponent<PlayerStats>().GainHealth(1);
            chadMeterValue = 0;
            Instantiate(enemySpawnerPrefab, Vector3.zero, Quaternion.identity);
            meterFullTriggered = false;
        }
    }

    private void PerformDash(Vector3 direction)
    {
        transform.Translate(direction.normalized * dashSpeed * Time.deltaTime, Space.World);
    }
}

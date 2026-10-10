using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private Transform healthIcons;

    //Player health variables
    private int maxHealth = 5;
    private float immuneTime = 1f;
    private float immuneTimer = 0f;

    private int currentHealth;
    private bool isImmune = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(isImmune)
        {
            immuneTimer += Time.deltaTime;
            if (immuneTimer >= immuneTime)
            {
                isImmune = false;
                immuneTimer = 0f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isImmune) return;
        if (currentHealth <= 0) return;

        if (currentHealth <= 1)
        {
            // Call gamemanager gameover method
            GameManager manager = FindAnyObjectByType<GameManager>();
            manager.GameOver();
        }

        SetImageAlpha();
        currentHealth -= damage;
        isImmune = true;
    }

    public void GainHealth(int health)
    {
        if (currentHealth >= maxHealth) return;
        currentHealth++;

        var image = healthIcons.GetChild(currentHealth - 1).GetComponent<Image>();
        Color tempColor = image.color;
        tempColor.a = 1f;
        image.color = tempColor;
    }

    /// <summary>
    /// Set image alpha on health lost
    /// </summary>
    private void SetImageAlpha()
    {
        // 1. Get the current color. -1 because it's an index in an array.
        var image = healthIcons.GetChild(currentHealth - 1).GetComponent<Image>();
        Color tempColor = image.color;

        // 2. Modify the alpha channel (0.0f to 1.0f)
        tempColor.a = 0.2f;

        // 3. Assign it back to the image
        image.color = tempColor;
    }
}

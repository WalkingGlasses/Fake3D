using UnityEngine;
using TMPro;

public class SurvivorPlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHP = 100f;
    public float currentHP = 100f;

    public float damageAmount = 5f;
    public float regenerationRate = 1f;
    
    [Header("Regeneration Delay")]
    public float regenerationDelay = 2f;
    private float regenerationTimer = 0f;

    [Header("Health UI")]
    public TMP_Text healthText;
    
    [Header("Hit Effect")]
    public SpriteRenderer playerRenderer;

    public float flashDuration = 0.4f;
    public float flashInterval = 0.08f;

    private float flashTimer = 0f;
    private Color originalColor;

    [Header("Game Over")] public GameObject gameOverUI;
    
    private bool isDead = false;

    private void Start()
    {
        currentHP = maxHP;

        UpdateHealthUI();
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.color;
        }
    }

    private void Update()
    {
        if (isDead) return;
        
        RegenerateHealth();
        UpdateHealthUI();
        UpdateHitFlash();
        
        Debug.Log("Checking HP: " + currentHP);

        if (currentHP <= 0)
            Die();
    }
    
    
    private void UpdateHitFlash()
    {
        if (playerRenderer == null) return;

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;

            int flashState = Mathf.FloorToInt(
                flashTimer / flashInterval
            );

            if (flashState % 2 == 0)
            {
                playerRenderer.color = Color.red;
            }
            else
            {
                playerRenderer.color = originalColor;
            }
        }
        else
        {
            playerRenderer.color = originalColor;
        }
    }


    private void RegenerateHealth()
    {
        if (regenerationTimer > 0f)//why did i not add this sooner i thought my math was wrong but my player was a mf Inaros Persistence Grace build with juiced armor and scarab armor on T - T
        {
            regenerationTimer -= Time.deltaTime;
            return;
        }

        if (currentHP < maxHP)
        {
            currentHP += regenerationRate * Time.deltaTime;
            currentHP = Mathf.Min(currentHP, maxHP);
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage called! Damage: " + damage);
        
        currentHP -= damage;

        currentHP = Mathf.Max(currentHP, 0f);
        
        regenerationTimer = regenerationDelay;
        
        flashTimer = flashDuration;

        UpdateHealthUI();

        Debug.Log("Player HP: " + currentHP);
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " +
                              Mathf.CeilToInt(currentHP) + " / " +
                              Mathf.CeilToInt(maxHP);
        }
    }

    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        Time.timeScale = 0f;
        
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogError("Game Over UI is not assigned!");
        }
    }
}
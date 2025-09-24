using System;
using UnityEngine;

public class PlayerHealthModel : MonoBehaviour
{
    [Header("Salud")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Protección al reaparecer")]
    [Tooltip("Invulnerabilidad al habilitar el objeto (reintentar/cargar escena)")]
    [SerializeField] private float spawnInvulnerability = 0.3f;

    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action OnDied;

    private bool isInvulnerable;
    private float invulnerableUntil;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
    
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void OnEnable()
    {
      
        ResetHealth();
        SetInvulnerable(spawnInvulnerability);
    }

    private void Update()
    {
        if (isInvulnerable && Time.unscaledTime >= invulnerableUntil)
            isInvulnerable = false;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        if (isInvulnerable) return;
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            OnDied?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        if (currentHealth <= 0) return;

        int prev = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (currentHealth != prev)
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetInvulnerable(float seconds)
    {
        if (seconds <= 0f) return;
        isInvulnerable = true;
        invulnerableUntil = Time.unscaledTime + seconds;
    }
}
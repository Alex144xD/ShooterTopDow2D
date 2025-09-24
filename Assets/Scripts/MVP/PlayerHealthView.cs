using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void Reset()
    {
        if (!healthSlider) healthSlider = GetComponentInChildren<Slider>(true);
    }

    private void Awake()
    {
        if (!healthSlider)
        {
            healthSlider = GetComponentInChildren<Slider>(true);
            if (!healthSlider)
                Debug.LogWarning($"[PlayerHealthView] Asigna un Slider en {name}.");
        }

        if (healthSlider) healthSlider.interactable = false;
    }

    public void SetMaxHealth(int maxHealth)
    {
        if (!healthSlider) return;
        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        if (!healthSlider) return;
        healthSlider.value = currentHealth;
    }
}
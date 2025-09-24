using System.Collections;
using UnityEngine;

public class PlayerHealthPresenter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealthModel model; 
    [SerializeField] private PlayerHealthView view;   

    private void Awake()
    {
        if (!model) model = GetComponent<PlayerHealthModel>();
        if (!view) view = FindObjectOfType<PlayerHealthView>(true);
    }

    private void OnEnable()
    {
        if (model != null)
        {
            model.OnHealthChanged += HandleHealthChanged;
            model.OnDied += HandleDied;

      
            if (model.CurrentHealth <= 0)
                model.ResetHealth();
        }

        ForceRefreshView();             
        StartCoroutine(DeferredRefresh()); 
    }

    private void Start()
    {
        ForceRefreshView();
    }

    private void OnDisable()
    {
        if (model != null)
        {
            model.OnHealthChanged -= HandleHealthChanged;
            model.OnDied -= HandleDied;
        }
    }

    private IEnumerator DeferredRefresh()
    {
        yield return null;
        ForceRefreshView();
    }

    public void TakeDamage(int amount) => model?.TakeDamage(amount);
    public void Heal(int amount) => model?.Heal(amount);

    public void ResetUIAndHealth()
    {
        if (model == null) return;
        model.ResetHealth();
        ForceRefreshView();
        StartCoroutine(DeferredRefresh());
    }

    public void ForceRefreshView()
    {
        if (model == null || view == null) return;
        view.SetMaxHealth(model.MaxHealth);
        view.UpdateHealth(model.CurrentHealth);
    }

    private void HandleHealthChanged(int current, int max)
    {
        if (view == null) return;
        view.SetMaxHealth(max);
        view.UpdateHealth(current);
    }

    private void HandleDied()
    {
        var ui = FindAnyObjectByType<GameOverUIController>();
        if (ui) ui.ShowGameOverScreen();
        Time.timeScale = 0f;
    }
}
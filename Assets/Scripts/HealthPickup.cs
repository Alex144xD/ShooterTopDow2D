using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthPresenter presenter = other.GetComponent<PlayerHealthPresenter>();
            if (presenter != null)
            {
                presenter.Heal(healAmount);
            }

            Destroy(gameObject); // Destruye el objeto de curación
        }
    }
}
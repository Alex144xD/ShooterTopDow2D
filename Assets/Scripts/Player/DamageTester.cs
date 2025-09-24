using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamageTester : MonoBehaviour
{
    private PlayerHealthPresenter presenter;

    private void Start()
    {
        presenter = GetComponent<PlayerHealthPresenter>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            presenter.TakeDamage(10); // Le quita 10 de vida al jugador
            Debug.Log("¡Jugador recibió daño!");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            presenter.Heal(5); // Cura 5 de vida al jugador
            Debug.Log("¡Jugador se curó!");
        }
    }
}


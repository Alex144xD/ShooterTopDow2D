using UnityEngine;
using UnityEngine.SceneManagement;

public class Empezar : MonoBehaviour
{
    public void empezar()
    {
        // OPCIONAL: Si quieres, reinicia el Score antes de cargar la escena
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        // Vuelve a poner la escala de tiempo en 1, por si estaba pausada
        Time.timeScale = 1f;

        // Carga la escena y todo lo que esté en la escena volverá a su estado inicial
        SceneManager.LoadScene("Prueba");
    }
}


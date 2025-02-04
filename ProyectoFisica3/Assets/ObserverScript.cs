using UnityEngine;

public class ObserverScript : MonoBehaviour
{
    public AudioSource ambulanceSiren;
    public Transform listener;  // Opcional para Doppler avanzado
    private AmbulanceScript ambulance;

    void Start()
    {
        // Corrección del método obsoleto
        ambulance = FindFirstObjectByType<AmbulanceScript>();

        if (ambulance != null)
        {
            ambulance.OnSpeedChanged += HandleSpeedChange;
        }
    }

    // Método manejador con parámetro correcto
    private void HandleSpeedChange(Vector2 newVelocity)
    {
        // Ajuste básico de tono
        float speedFactor = newVelocity.magnitude / 20f;
        ambulanceSiren.pitch = Mathf.Clamp(speedFactor, 0.8f, 1.2f);

        // Opcional: Efecto Doppler avanzado
        if (listener != null)
        {
            Vector2 relativeVelocity = newVelocity - (Vector2)listener.position.normalized;
            float dopplerFactor = 1 + Vector2.Dot(relativeVelocity, (listener.position - transform.position).normalized) / 343f;
            ambulanceSiren.pitch = Mathf.Clamp(dopplerFactor, 0.5f, 2f);
        }
    }

    void OnDestroy()
    {
        if (ambulance != null)
        {
            ambulance.OnSpeedChanged -= HandleSpeedChange;  // Desuscribir al destruir
        }
    }
}
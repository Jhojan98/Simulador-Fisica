// ObserverScript.cs
using UnityEngine;
using System.Collections.Generic;

public class ObserverScript : MonoBehaviour
{
    public AudioSource ambulanceSiren;
    public AmbulanceScript ambulance;
    private List<Wave> activeWaves = new List<Wave>();

    void Start()
    {
        ambulance = FindFirstObjectByType<AmbulanceScript>();
        if (ambulance != null)
        {
            ambulance.OnSpeedChanged += HandleSpeedChange;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Wave wave = other.GetComponent<Wave>();
        if (wave != null)
        {
            activeWaves.Add(wave);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Wave wave = other.GetComponent<Wave>();
        if (wave != null)
        {
            activeWaves.Remove(wave);
        }
    }

    void Update()
    {
        float totalFrequency = 0f;
        int waveCount = 0;

        // Obtener la posición y velocidad del observador (usando 'velocity' si es posible)
        Rigidbody2D rbObserver = GetComponent<Rigidbody2D>();
        Vector2 observerPos = transform.position;
        Vector2 observerVel = rbObserver != null ? rbObserver.linearVelocity : Vector2.zero;

        foreach (Wave wave in activeWaves)
        {
            // Se asume que GetObservedFrequency implementa la fórmula del efecto Doppler
            float observedFreq = wave.GetObservedFrequency(observerPos, observerVel);
            totalFrequency += observedFreq;
            waveCount++;
        }

        if (waveCount > 0)
        {
            float avgFrequency = totalFrequency / waveCount;
            Debug.Log("Frecuencia Observada: " + avgFrequency + " - Frecuencia Base: " + ambulance.sourceFrequency);

            // Relación de frecuencias (observada / fuente) para determinar el pitch
            ambulanceSiren.pitch = avgFrequency / ambulance.sourceFrequency;
        }
    }

    void OnDestroy()
    {
        if (ambulance != null)
        {
            ambulance.OnSpeedChanged -= HandleSpeedChange;
        }
    }

    private void HandleSpeedChange(Vector2 newVelocity)
    {
        // Opcional: Ajustes adicionales si es necesario
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Diagnostics;

public class ObserverScript : MonoBehaviour
{
    public AudioSource ambulanceSiren;
    public AmbulanceScript ambulance;
    private List<Wave> activeWaves = new List<Wave>();
    private float i = 1;

    public Text frecuenciaText;

    void Start()
    {
        // Buscar la ambulancia en la escena
        ambulance = FindFirstObjectByType<AmbulanceScript>();
        

        if (ambulance != null)
        {
            ambulance.OnSpeedChanged += HandleSpeedChange;
        }
    }

    // Cuando una onda entra en el collider del observador se agrega a la lista
    void OnTriggerEnter2D(Collider2D other)
    {
        
               
        Wave wave = other.GetComponent<Wave>();
        if (wave != null)
        {
            UnityEngine.Debug.Log("Me toco" + i);
            i++;
            activeWaves.Add(wave);
            //foreach (Wave wavee in activeWaves)
            //{
            //    UnityEngine.Debug.Log("-----------------------------------------------");
            //    UnityEngine.Debug.Log($"Wave: {wavee.id}");
            //    UnityEngine.Debug.Log("-----------------------------------------------");
            //}
        }
       
    }

    // Cuando la onda sale, se elimina de la lista
    void OnTriggerExit2D(Collider2D other)
    {
        //UnityEngine.Debug.Log("Salio" + i);
        Wave wave = other.GetComponent<Wave>();
        if (wave != null)
        {
            activeWaves.Remove(wave);
        }
    }

    void Update()
    {
        // Eliminar de la lista las ondas que han sido destruidas
        for (int i = activeWaves.Count - 1; i >= 0; i--)
        {
            if (activeWaves[i] == null)
            {
                activeWaves.RemoveAt(i);
            }
        }

        float totalFrequency = 0f;
        int waveCount = 0;

        // Obtener la posición y velocidad del observador
        Rigidbody2D rbObserver = GetComponent<Rigidbody2D>();
        Vector2 observerPos = transform.position;
        Vector2 observerVel = rbObserver != null ? rbObserver.linearVelocity : Vector2.zero;

        // Calcular la frecuencia observada de cada onda
        foreach (Wave wave in activeWaves)
        {
            float observedFreq = wave.GetObservedFrequency(observerPos, observerVel);
            totalFrequency += observedFreq;
            waveCount++;
            // aqui hay un problema ya que por ejemplo la frecucencia no esta siendo tomada correctamente, ademas que no se por que se crea una lista de waves activas
        }

        if (waveCount > 0)
        {
            float avgFrequency = totalFrequency / waveCount;
            UnityEngine.Debug.Log("Frecuencia Observada: " + avgFrequency + " - Frecuencia Base: " + ambulance.sourceFrequency);
            // Se añade la etiqueta "Hz" al texto para indicar la unidad
            frecuenciaText.text = avgFrequency.ToString("F2") + " Hz";
            // Ajusta el pitch del AudioSource según la relación observada vs. base
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
        // Aquí se pueden implementar ajustes adicionales si es necesario
    }
}

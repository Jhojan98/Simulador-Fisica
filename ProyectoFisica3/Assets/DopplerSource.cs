using UnityEngine;

public class DopplerSource : MonoBehaviour
{
    public float waveSpeed = 343f; // Velocidad del sonido
    public float baseFrequency = 440f; // Frecuencia base (ej: 440 Hz = La4)
    public AudioSource audioSource; // Componente de audio
                                   

    private Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource.pitch = 1f; // Pitch inicial
    }


    public float CalculatePerceivedFrequency(Vector3 observerPosition)
    {
        Vector3 sourceVelocity = rb.linearVelocity; // Usar linearVelocity
        Vector3 directionToObserver = (observerPosition - transform.position).normalized;

        // Fórmula corregida (considerando dirección relativa)
        float vs = Vector3.Dot(sourceVelocity, directionToObserver);
        float perceivedFreq = baseFrequency * (waveSpeed / (waveSpeed - vs));

        return perceivedFreq;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

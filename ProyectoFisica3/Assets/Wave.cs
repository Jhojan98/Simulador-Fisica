using UnityEngine;

public class Wave : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    [Header("Propiedades de la Onda")]
    public float waveSpeed = 0.5f;      // Velocidad de expansión (v)
    public float maxRadius = 2f;     // Radio máximo antes de destruirse
    public float wavelength = 1f;     // Longitud de onda (λ)
    public float frequency = 2f;      // Frecuencia (f)

    private Vector2 origin;           // Posición inicial de la fuente
    private float currentRadius = 0f;
    private Vector2 sourceVelocity; // Velocidad de la fuente al emitir la onda

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        origin = transform.position;
        // Crear instancia única del material para evitar conflicto entre ondas
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.material = new Material(renderer.material);
        }
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /// Expansión de la onda
        currentRadius += waveSpeed * Time.deltaTime;
        transform.localScale = Vector2.one * currentRadius * 2;

        // Actualizar material
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.material.SetFloat("_WaveScale", currentRadius / wavelength);
        }

        // Destruir al alcanzar el radio máximo
        if (currentRadius >= maxRadius)
        {
            Destroy(gameObject);
        }
    }

    // Método para recibir la velocidad de la fuente
    public void SetSourceVelocity(Vector2 velocity)
    {
        sourceVelocity = velocity;
    }

    // Cálculo de frecuencia observada
    public float GetObservedFrequency(Vector2 observerPosition, Vector2 observerVelocity)
    {
        Vector2 directionToObserver = (observerPosition - origin).normalized;
        float sourceSpeed = Vector2.Dot(sourceVelocity, directionToObserver);
        float observerSpeed = Vector2.Dot(observerVelocity, -directionToObserver); // Velocidad del observador hacia la fuente
        return frequency * (waveSpeed + observerSpeed) / (waveSpeed - sourceSpeed);
    }
}

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
    private CircleCollider2D waveCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveCollider = GetComponent<CircleCollider2D>();
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

        // Actualizar el radio del collider
        if (waveCollider != null)
        {
            waveCollider.radius = currentRadius;
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
        float denominator = waveSpeed - sourceSpeed;
        if (Mathf.Approximately(denominator, 0f))
        {
            // Evitamos la división por cero. Puedes decidir un valor por defecto o ajustar la lógica.
            denominator = 0.0001f;
        }
        return frequency * (waveSpeed + observerSpeed) / (waveSpeed - sourceSpeed);
    }
}

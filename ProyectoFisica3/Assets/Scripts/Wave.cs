using UnityEngine;

public class Wave : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;

    [Header("Propiedades de la Onda")]
    public float waveSpeed = 0.5f;      // Velocidad de propagación de la onda
    public float maxRadius = 2f;        // Radio máximo antes de destruirse
    public float wavelength = 1f;       // Longitud de onda (λ)
    public float frequency = 0f;        // Frecuencia base (f)
    public float id;


    private Vector2 origin;             // Posición de emisión (instante de creación)
    private float currentRadius = 0f;
    private Vector2 sourceVelocity;     // Velocidad de la fuente en el instante de emisión
    private CircleCollider2D waveCollider;

    void Start()
    {
        waveCollider = GetComponent<CircleCollider2D>();
        origin = transform.position;

        // Crear una instancia única del material para evitar conflictos entre ondas
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.material = new Material(renderer.material);
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Expansión de la onda
        currentRadius += waveSpeed * Time.deltaTime;
        transform.localScale = Vector2.one * currentRadius * 2;

        // Actualizar material para efectos visuales (por ejemplo, se puede usar _WaveScale en un shader)
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.material.SetFloat("_WaveScale", currentRadius / wavelength);
        }

        // Actualizar el radio del collider para que coincida con la onda visual
        if (waveCollider != null)
        {
            waveCollider.radius = currentRadius;
        }

        // Destruir la onda cuando alcance el radio máximo
        if (currentRadius >= maxRadius)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Establece la velocidad de la fuente en el instante de emisión.
    /// </summary>
    public void SetSourceVelocity(Vector2 velocity)
    {
        sourceVelocity = velocity;
    }

    /// <summary>
    /// Calcula la frecuencia observada aplicando la fórmula Doppler:
    /// f' = f * (v + v_obs) / (v - v_fuente)
    /// donde se usan las componentes de velocidad en la dirección de la línea de visión.
    /// </summary>
    public float GetObservedFrequency(Vector2 observerPosition, Vector2 observerVelocity)
    {
        Vector2 toObserver = observerPosition - origin;
        // Si el observador coincide con la posición de emisión, se retorna la frecuencia base
        if (toObserver == Vector2.zero)
        {
            return frequency;
        }

        Vector2 directionToObserver = toObserver.normalized;
        // Componente de la velocidad de la fuente hacia el observador
        float sourceSpeedComponent = Vector2.Dot(sourceVelocity, directionToObserver);
        // Componente de la velocidad del observador hacia la fuente
        float observerSpeedComponent = Vector2.Dot(observerVelocity, -directionToObserver);

        float denominator = waveSpeed - sourceSpeedComponent;
        // Evitar división por cero o denominador muy pequeño
        if (Mathf.Abs(denominator) < 0.001f)
        {
            Debug.LogWarning($"[Wave {id}] Denominador casi nulo. Se usa la frecuencia base.");
            return frequency;
        }

        float observedFrequency = frequency * (waveSpeed + observerSpeedComponent) / denominator;
        Debug.Log($"[Wave {id}] Frecuencia Observada: {observedFrequency:F2} Hz  " +
                  $"(Componente Fuente: {sourceSpeedComponent:F2}, Componente Observador: {observerSpeedComponent:F2})");
        return observedFrequency;
    }
}

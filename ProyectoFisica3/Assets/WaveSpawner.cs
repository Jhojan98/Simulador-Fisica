using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Configuraci�n de Spawn")]
    public GameObject wavePrefab;
    public float spawnInterval = 1f;  // Tiempo entre ondas
    public float sourceFrequency = 1f;
    public float waveSpeed = 5f;
    public float maxRadius = 2f; // Controlar desde el Spawner

    private float timer = 0f;
    public Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnWave();
            timer = 0f;
        }

        // Movimiento de la fuente (ejemplo con WASD)
        float moveSpeed = 3f;
        Vector2 movement = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        ) * moveSpeed;
        rb.linearVelocity = movement;
    }

    void SpawnWave()
    {
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        Wave waveController = wave.GetComponent<Wave>();
        // Configurar propiedades
        waveController.frequency = sourceFrequency;
        waveController.waveSpeed = waveSpeed;
        waveController.maxRadius = maxRadius;

        // Pasar velocidad actual de la fuente a la onda
        waveController.SetSourceVelocity(rb.linearVelocity);
    }

}

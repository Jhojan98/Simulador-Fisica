using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject wavePrefab;
    public float spawnInterval = 1f;    // Tiempo entre emisiones de onda
    public float sourceFrequency = 1f;  // Frecuencia base de la fuente
    public float waveSpeed = 5f;        // Velocidad de propagación de la onda
    public float maxRadius = 2f;        // Radio máximo de la onda
    public float moveSpeed = 0f;        // Velocidad de la fuente (ambulancia)
    public AmbulanceScript ambulance;   // Referencia al script de la ambulancia
    private float i;
    private float timer = 0f;
    public Rigidbody2D rb;

    public Button button;
    public Slider speedAmbulanceSlider;
    public Slider speedWaveSlider;
    public Slider frecWavesSlider;
    public Text speedAmbulanceText;
    public Text speedWavesText;
    public Text frecWavesText;


    // Factores de escala para convertir valores reales a simulados
    public float frequencyScale = 0.1f; // Si el slider indica 10, la frecuencia simulada será 10*0.1 = 1 Hz
    public float waveSpeedFactor = 0.1f;  // Si el slider indica 50, la onda se moverá a 50*0.1 = 5 unidades/s

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Agregar listeners una sola vez en Start
        button.onClick.AddListener(ResetPositionAndVelocity);
        speedAmbulanceSlider.onValueChanged.AddListener(SetVelocityAmbulance);
        speedWaveSlider.onValueChanged.AddListener(SetVelocityWaves);
        frecWavesSlider.onValueChanged.AddListener(SetFrecWave);

        // Inicializar valores de sliders y textos
        speedAmbulanceSlider.value = moveSpeed;
        speedWaveSlider.value = waveSpeed;
        frecWavesSlider.value = 1f / spawnInterval;

        // Actualizar textos con unidades
        speedAmbulanceText.text = speedAmbulanceSlider.value.ToString("F2") ;
        speedWavesText.text = speedWaveSlider.value.ToString("F2") + " u/s";
        frecWavesText.text = frecWavesSlider.value.ToString("F2") + " Hz";
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval && waveSpeed != 0)
        {
            SpawnWave();
            timer = 0f;
        }

        // Actualizar los textos de los sliders con sus unidades
        speedAmbulanceText.text = speedAmbulanceSlider.value.ToString("F2")  ;
        speedWavesText.text = speedWaveSlider.value.ToString("F2") + " u/s";
        frecWavesText.text = frecWavesSlider.value.ToString("F2") + " Hz";

        // Sincronizar la velocidad del spawner con la de la ambulancia
        rb.linearVelocity = ambulance.rb.linearVelocity;
    }

    // Resetea la posición y velocidad de la fuente
    Vector2 newPosition = new Vector2(-10f, 0f);
    void ResetPositionAndVelocity()
    {
        rb.position = newPosition;
        rb.linearVelocity = Vector2.zero; // Detiene el movimiento
        ambulance.moveSpeed = 0f;
        ambulance.ResetPositionAndVelocity(newPosition);
        speedAmbulanceSlider.value = 0f;
    }

    // Ajusta el intervalo de spawn basado en la frecuencia seleccionada
    void SetFrecWave(float sliderValue)

    {
        float simulationFrequency = sliderValue * frequencyScale;
        sourceFrequency = simulationFrequency;
        if (simulationFrequency != 0)
            spawnInterval = 1f / simulationFrequency;
    }

    // Ajusta la velocidad de propagación de la onda
    void SetVelocityWaves(float velocityWaves)
    {
        waveSpeed = velocityWaves * waveSpeedFactor;
    }

    // Ajusta la velocidad de la ambulancia
    void SetVelocityAmbulance(float newSpeed)
    {
        ambulance.moveSpeed = newSpeed;
    }

    // Crea una nueva onda y le pasa la velocidad actual de la fuente (en el instante de emisión)
    void SpawnWave()
    {
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        Wave waveController = wave.GetComponent<Wave>();

        // Configurar propiedades de la onda
        waveController.frequency = sourceFrequency;
        waveController.waveSpeed = waveSpeed;
        waveController.maxRadius = maxRadius;
        waveController.id = i;
        i++;

        // Almacenar la velocidad de la fuente en el momento de la emisión
        waveController.SetSourceVelocity(rb.linearVelocity);
        Debug.Log($"Spawned Wave {waveController.id} en posición {transform.position} con velocidad fuente {rb.linearVelocity}");
    }
}

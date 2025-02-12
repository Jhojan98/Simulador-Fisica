using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [Header("Configuraci�n de Spawn")]
    public GameObject wavePrefab;
    public float spawnInterval = 1f;  // Tiempo entre ondas
    public float sourceFrequency = 1f;
    public float waveSpeed = 5f;
    public float maxRadius = 2f;// Controlar desde el Spawner
    public float moveSpeed = 0f;
    public AmbulanceScript ambulance; // Referencia al script de la ambulancia


    private float timer = 0f;
    public Rigidbody2D rb;

    public Button button;
    public Slider speedAmbulanceSlider;
    public Slider speedWaveSlider;
    public Slider frecWavesSlider;
    public Text speedAmbulanceText;
    public Text speedWavesText;
    public Text frecWavesText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        button.onClick.AddListener(ResetPositionAndVelocity);

        speedAmbulanceSlider.value = moveSpeed;
        speedWaveSlider.value = waveSpeed;
        frecWavesSlider.value = 1/spawnInterval;

        speedAmbulanceText.text =  speedAmbulanceSlider.value.ToString("F2");
        speedWavesText.text = speedWaveSlider.value.ToString("F2");
        frecWavesText.text = frecWavesSlider.value.ToString("F2");
       
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
        button.onClick.AddListener(ResetPositionAndVelocity);
        // slider Ambulance
        speedAmbulanceSlider.onValueChanged.AddListener(SetVelocityAmbulance);
        speedAmbulanceText.text =  speedAmbulanceSlider.value.ToString("F2");

        //slider Waves
        speedWaveSlider.onValueChanged.AddListener(SetVelocityWaves);
        speedWavesText.text = speedWaveSlider.value.ToString("F2");

        // slider frec waves
        frecWavesSlider.onValueChanged.AddListener(SetFrecWave);
        frecWavesText.text = frecWavesSlider.value.ToString("F2");

        // Sincronizar la velocidad con la ambulancia
        rb.linearVelocity = ambulance.rb.linearVelocity;
        
    }

    Vector2 newPosition = new Vector2(-10f, 0f);
    void ResetPositionAndVelocity()
    {
        rb.position = newPosition;
        rb.linearVelocity = newPosition; // Detener el movimiento
        ambulance.moveSpeed = 0f;
        ambulance.ResetPositionAndVelocity(newPosition);
        speedAmbulanceSlider.value = 0f;

    }

    void SetFrecWave(float frecWave)
    {
        spawnInterval = 1/frecWave;
    }
    void SetVelocityWaves(float velocityWaves)
    {
        waveSpeed = velocityWaves;
    }
    void SetVelocityAmbulance(float newSpeed)
    {
        ambulance.moveSpeed = newSpeed;
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

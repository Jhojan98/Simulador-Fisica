using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
public class AmbulanceScript : MonoBehaviour
{
    public float moveSpeed = 0f; // Velocidad ajustable
    public Rigidbody2D rb;
    // Evento para notificar cambios de velocidad
    public delegate void SpeedChangedHandler(Vector2 newVelocity);
    public event SpeedChangedHandler OnSpeedChanged;
    public float sourceFrequency = 1f;
    public float simulationSpeedFactor = 0.1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Desactivar la gravedad para este objeto
        rb.gravityScale = 0f;

        // Asegurarse de que no haya restricciones de rotación
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    public void ResetPositionAndVelocity(Vector2 newPosition)
    {
        transform.position = newPosition;
        rb.linearVelocity = newPosition;
        rb.angularVelocity = 0f;
        OnSpeedChanged?.Invoke(Vector2.zero);
    }
    void Update()
    {
        // transform.position = transform.position + (Vector3.right * moveSpeed) * Time.deltaTime;
        rb.linearVelocity = Vector3.right * moveSpeed*simulationSpeedFactor; // Mueve usando velocidad física
        // Evento para notificar cambios de velocidad
        Vector2 newVelocity = Vector2.right * moveSpeed;
        if (rb.linearVelocity != newVelocity)
        {
            rb.linearVelocity = newVelocity;
            // Notificar a los observadores
            OnSpeedChanged?.Invoke(newVelocity);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
public class AmbulanceScript : MonoBehaviour
{
    public float moveSpeed = 20f; // Velocidad ajustable
    public Rigidbody2D rb;
    // Evento para notificar cambios de velocidad
    public delegate void SpeedChangedHandler(Vector2 newVelocity);
    public event SpeedChangedHandler OnSpeedChanged;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Desactivar la gravedad para este objeto
        rb.gravityScale = 0f;

        // Asegurarse de que no haya restricciones de rotación
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = transform.position + (Vector3.right * moveSpeed) * Time.deltaTime;
        rb.linearVelocity = Vector3.right * moveSpeed; // Mueve usando velocidad física
        /* float moveSpeed = 5f;
        Vector2 movement = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        ) * moveSpeed;
        rb.linearVelocity = movement;*/
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

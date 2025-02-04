using UnityEngine;

public class AmbulanceScript : MonoBehaviour
{
    public float moveSpeed = 20f; // Velocidad ajustable
    public Rigidbody2D rb;

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
    }
}

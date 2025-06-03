using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneController : MonoBehaviour
{
    public float forwardSpeed = 20f;
    public float pitchSpeed = 45f;
    public float rollSpeed = 50f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.None;
    }

    void FixedUpdate()
    {
        // INPUT – Yön tuşları
        float pitchInput = 0f;
        float rollInput = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) pitchInput = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) pitchInput = -1f;
        if (Input.GetKey(KeyCode.LeftArrow)) rollInput = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) rollInput = 1f;

        // ROTASYON – Uçağı döndür (pitch ve roll)
        Quaternion deltaRotation = Quaternion.Euler(
            pitchInput * pitchSpeed * Time.fixedDeltaTime,
            0f,
            -rollInput * rollSpeed * Time.fixedDeltaTime
        );
        rb.MoveRotation(rb.rotation * deltaRotation);

        // YÖN DEĞİŞTİRME – Roll’e göre yönü güncelle (YAW olmadan!)
        Vector3 direction = transform.forward;
        Vector3 tilt = Vector3.Cross(transform.up, Vector3.up);
        direction += tilt * Time.fixedDeltaTime;
        direction.Normalize();

        // İLERİ HAREKET
        rb.MovePosition(rb.position + direction * forwardSpeed * Time.fixedDeltaTime);

        // İSTİKAMETE BAK
        rb.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}

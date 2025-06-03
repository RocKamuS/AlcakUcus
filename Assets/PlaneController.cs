using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneController : MonoBehaviour
{
    public float forwardSpeed = 20f;
    public float pitchSpeed = 45f;
    public float rollSpeed = 50f;
    // Bank turn hızını ayarlamak için çarpan
    public float bankTurnSpeed = 2f;

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

        // Yeni rotasyonu hesapla, böylece yön güncellenirken güncel açılar kullanılır
        Quaternion newRotation = rb.rotation * deltaRotation;

        // YÖN DEĞİŞTİRME – Roll’e göre yönü güncelle (YAW olmadan!)
        Vector3 direction = newRotation * Vector3.forward;
        Vector3 tilt = Vector3.Cross(newRotation * Vector3.up, Vector3.up);

        // Roll açısı yön değişimine etki etsin
        direction += tilt * bankTurnSpeed * Time.fixedDeltaTime;

        direction += tilt * Time.fixedDeltaTime;

        direction.Normalize();

        // İLERİ HAREKET
        rb.MovePosition(rb.position + direction * forwardSpeed * Time.fixedDeltaTime);


        // Bank acisina bagli olarak burunu yeni yone cevir
        Quaternion lookRotation = Quaternion.LookRotation(direction, newRotation * Vector3.up);
        rb.MoveRotation(lookRotation);

        // Hesaplanan rotasyonu uygula
        rb.MoveRotation(newRotation);
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneController : MonoBehaviour
{
    public float forwardSpeed = 20f;
    public float pitchSpeed = 45f;
    public float rollSpeed = 50f;
    // Bank turn hızını ayarlamak için çarpan
    // Sağa/sola dönüşlerde uçağın daha hızlı yön değiştirebilmesi için
    // bankeden elde edilen dönüş hızını artırıyoruz.
    public float bankTurnSpeed = 50f;

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

        // Roll miktarına bağlı olarak yatış dönüşü (yaw)
        // daha doğrudan rotasyona uygula.
        float bankAmount = Vector3.Dot(newRotation * Vector3.right, Vector3.up);
        Quaternion yawDelta = Quaternion.Euler(
            0f,
            bankAmount * bankTurnSpeed * Time.fixedDeltaTime,
            0f
        );
        newRotation = yawDelta * newRotation;

        // YÖN DEĞİŞTİRME – Güncellenmiş rotasyonla yeni yönü hesapla
        Vector3 direction = newRotation * Vector3.forward;
        direction.Normalize();

        // İLERİ HAREKET
        rb.MovePosition(rb.position + direction * forwardSpeed * Time.fixedDeltaTime);


        // Bank açısına bağlı olarak burunu yeni yöne çevir
        Quaternion lookRotation = Quaternion.LookRotation(direction, newRotation * Vector3.up);
        rb.MoveRotation(lookRotation);
    }
}

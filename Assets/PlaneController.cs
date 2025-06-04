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
    // Doğrudan yön tuşları ile uygulanacak yaw hızı
    public float yawSpeed = 45f;
    // Yön tuşları bırakıldığında uçağı kendiliğinden düzleme hızı
    public float autoLevelSpeed = 2f;

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
        float yawInput = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) pitchInput = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) pitchInput = -1f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rollInput = -1f;
            yawInput = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rollInput = 1f;
            yawInput = 1f;
        }

        // Yön tuşlarına basılmadığında yavaşça düzleşme
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            float bankAmount = Vector3.Dot(rb.rotation * Vector3.right, Vector3.up);
            rollInput = -bankAmount * autoLevelSpeed;
        }

        // ROTASYON – Uçağı döndür (pitch ve roll)
        Quaternion deltaRotation = Quaternion.Euler(
            pitchInput * pitchSpeed * Time.fixedDeltaTime,
            yawInput * yawSpeed * Time.fixedDeltaTime,
            -rollInput * rollSpeed * Time.fixedDeltaTime
        );

        // Yeni rotasyonu hesapla, böylece yön güncellenirken güncel açılar kullanılır
        Quaternion newRotation = rb.rotation * deltaRotation;

        // YÖN DEĞİŞTİRME – Sadece bank açısına göre yönü güncelle
        Vector3 direction = newRotation * Vector3.forward;

        // Uçağın sağ/sol yatıklık miktarı (roll). Sadece roll etkisini ölçer,
        // pitch sırasında sıfır olur böylece istenmeyen sapmalar engellenir.
        float bankAmount = Vector3.Dot(newRotation * Vector3.right, Vector3.up);

        // Bank açısına bağlı olarak uçağı dünya yukarı ekseni etrafında hafifçe
        // döndürerek yeni yönü oluştur
        Quaternion yawRotation = Quaternion.Euler(
            0f,
            -bankAmount * bankTurnSpeed * Time.fixedDeltaTime,
            0f
        );
        direction = yawRotation * direction;
        direction.Normalize();

        // İLERİ HAREKET
        rb.MovePosition(rb.position + direction * forwardSpeed * Time.fixedDeltaTime);


        // Bank açısına bağlı olarak burunu yeni yöne çevir
        Quaternion lookRotation = Quaternion.LookRotation(direction, newRotation * Vector3.up);
        rb.MoveRotation(lookRotation);
    }
}

using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    // Sahnedeki elle ayarlanmýþ kamera pozisyonunun, uçaða göre offset deðeri
    public Vector3 fixedOffset = new Vector3(1.5f, -1f, -15f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Offset pozisyonunu hedefin pozisyonuna göre belirle
        Vector3 desiredPosition = target.position +
            target.right * fixedOffset.x +
            target.up * fixedOffset.y +
            target.forward * fixedOffset.z;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Kamerayý hedefe odakla (biraz üstten bakmak istersen + Vector3.up * 0.5f gibi ekleme yapabilirsin)
        transform.LookAt(target.position);
    }
}

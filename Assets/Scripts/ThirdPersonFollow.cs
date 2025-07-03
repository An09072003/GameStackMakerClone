using UnityEngine;

public class ThirdPersonFollow : MonoBehaviour
{
    public Transform target;         // Nhân vật để follow
    public Vector3 offset = new Vector3(0, 10, -10);  // Đặt camera ở sau và trên
    public float smoothSpeed = 5f;   // Mượt mà

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // Camera luôn nhìn xuống 1 góc cố định, không bị xoay Y
        transform.rotation = Quaternion.Euler(45f, 0f, 0f); // Xoay xuống 45 độ, Y luôn bằng 0
    }
}

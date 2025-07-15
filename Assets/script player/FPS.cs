using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;   // n?u b?n ðang dùng Input System m?i
#endif

/// <summary>
/// Ði?u khi?n góc nh?n FPS:  
/// - Chu?t ngang (Mouse X) xoay thân ngý?i (Yaw)  
/// - Chu?t d?c (Mouse Y) xoay camera (Pitch)  
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    [Tooltip("Ð? nh?y chu?t t?ng")]
    public float mouseSensitivity = 100f;

    [Header("References")]
    [Tooltip("Transform c?a Player (object có CharacterController).")]
    public Transform playerBody;          // c?n gán trong Inspector

    float pitch = 0f;                     // xoay d?c (ng?ng / cúi)

    void Start()
    {
        // ?n & khóa con tr? chu?t vào gi?a màn h?nh
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Auto?fill n?u quên gán
        if (playerBody == null)
            playerBody = transform.parent;        // camera là child c?a player
    }

    void Update()
    {
        /* -------- 1. L?y input chu?t -------- */
#if ENABLE_INPUT_SYSTEM   // (Input System package m?i)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;
#else                     // (Input Manager c?)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
#endif

        /* -------- 2. Tính pitch (ng?ng?cúi) cho Camera -------- */
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);    // gi?i h?n không g?y c?

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        /* -------- 3. Xoay yaw (trái?ph?i) cho Player -------- */
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
    }
}

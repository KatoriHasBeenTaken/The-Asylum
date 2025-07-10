using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera cam;
    public float normalFOV = 60f;
    public float zoomInFOV = 40f;    // Khi nhìn th?y ma
    public float zoomOutFOV = 80f;   // Khi b? ?u?i
    public float transitionSpeed = 5f;

    private float targetFOV;

    void Start()
    {
        // Gán Camera n?u ch?a thi?t l?p s?n
        if (cam == null)
            cam = GetComponent<Camera>();

        targetFOV = normalFOV;
    }

    void Update()
    {
        // Làm m??t FOV khi thay ??i
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * transitionSpeed);
    }

    // G?i khi nhìn th?y ma
    public void ZoomIn()
    {
        targetFOV = zoomInFOV;
    }

    // G?i khi b? r??t
    public void ZoomOut()
    {
        targetFOV = zoomOutFOV;
    }

    // G?i khi ?ã an toàn ho?c tr? l?i bình th??ng
    public void ResetZoom()
    {
        targetFOV = normalFOV;
    }

    // (Tùy ch?n) Test nhanh b?ng phím b?m
    void DebugKeyTest()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ZoomIn();
        if (Input.GetKeyDown(KeyCode.Alpha2)) ZoomOut();
        if (Input.GetKeyDown(KeyCode.Alpha3)) ResetZoom();
    }
}

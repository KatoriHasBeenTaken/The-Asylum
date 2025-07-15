using UnityEngine;

public class GhostTrigger : MonoBehaviour
{
    private CameraEffects cameraEffects;

    void Start()
    {
        cameraEffects = Camera.main.GetComponent<CameraEffects>();
        Debug.Log("CameraEffects gán thành công: " + (cameraEffects != null));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ghost"))
        {
            Debug.Log("Player ?ã g?p ma!");
            cameraEffects.ZoomIn();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ghost"))
        {
            Debug.Log("Player khi gap ma!");
            cameraEffects.ResetZoom();
        }
    }
}

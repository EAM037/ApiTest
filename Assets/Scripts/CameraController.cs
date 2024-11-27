using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera1; // Asigna tu primera cámara en el Inspector
    public Camera camera2; // Asigna tu segunda cámara en el Inspector
    public KeyCode toggleKey = KeyCode.C; // Tecla para alternar cámaras (puedes cambiarla en el Inspector)

    void Start()
    {
        // Asegúrate de que solo una cámara esté activa al inicio
        if (camera1 != null && camera2 != null)
        {
            camera1.enabled = true;
            camera2.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleCameras();
        }
    }

    void ToggleCameras()
    {
        if (camera1 != null && camera2 != null)
        {
            camera1.enabled = !camera1.enabled;
            camera2.enabled = !camera2.enabled;
        }
    }
}

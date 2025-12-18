using UnityEngine;
using UnityEngine.InputSystem;

public class LimitedMouseLookAroundStart : MonoBehaviour
{
    [Header("Look settings")]
    public float sensitivity = 3f;

    [Header("Look limits (rond start)")]
    public float yawLimit = 60f;
    public float pitchUp = 25f;
    public float pitchDown = 25f;

    [Header("Zoom settings")]
    public float zoomSpeed = 10f;
    public float minFov = 40f;
    public float maxFov = 70f;

    private float yaw;
    private float pitch;

    private float startYaw;
    private float startPitch;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        Vector3 e = transform.eulerAngles;
        startYaw = NormalizeAngle(e.y);
        startPitch = NormalizeAngle(e.x);

        yaw = startYaw;
        pitch = startPitch;

        // veiligheid: start-FOV binnen limits
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFov, maxFov);
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // ===== LOOK (linkermuisknop) =====
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();

            yaw   += delta.x * sensitivity * Time.deltaTime;
            pitch -= delta.y * sensitivity * Time.deltaTime;

            yaw = Mathf.Clamp(yaw, startYaw - yawLimit, startYaw + yawLimit);
            pitch = Mathf.Clamp(pitch, startPitch - pitchDown, startPitch + pitchUp);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // ===== ZOOM (scroll wheel) =====
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.fieldOfView -= scroll * zoomSpeed * Time.deltaTime;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFov, maxFov);
        }
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}



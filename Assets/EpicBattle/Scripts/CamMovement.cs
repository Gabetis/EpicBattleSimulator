using UnityEngine;

public enum CameraState
{
    Free,
    Moving,
    Zooming
}

public class CamMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private float speed = 10f;

    [Header("Camera Limit")]
    [SerializeField] private float minX = -35f;
    [SerializeField] private float maxX = 26f;
    [SerializeField] private float minZ = -45f;
    [SerializeField] private float maxZ = 17f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;

    private CameraState state = CameraState.Free;

    private void Update()
    {
        HandleMovement();

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseZoom();
#else
        HandleTouchZoom();
#endif
    }

    public bool IsCameraFree()
    {
       return state == CameraState.Free;
    }

    private void HandleMovement()
    {
        Vector2 input = new Vector2(
            joystick.Horizontal,
            joystick.Vertical
        );

#if UNITY_EDITOR || UNITY_STANDALONE
        if (input.sqrMagnitude <= 0.01f)
            return;

        Move(input);
#else
        HandleTouchMovement(input);
#endif
    }

    private void HandleTouchMovement(Vector2 input)
    {
        bool moving = input.sqrMagnitude > 0.01f;

        if (state == CameraState.Zooming)
            return;

        if (state == CameraState.Free && moving)
        {
            state = CameraState.Moving;
        }

        if (state == CameraState.Moving)
        {
            if (moving)
            {
                Move(input);
            }
        }

        if (!moving && state == CameraState.Moving)
        {
            if (Input.touchCount == 0)
            {
                state = CameraState.Free;
            }
        }
    }

    private void Move(Vector2 input)
    {
        Vector3 movement = new Vector3(
            input.x,
            0f,
            input.y
        ) * speed * Time.deltaTime;

        Vector3 targetPosition = transform.position + movement;

        targetPosition.x = Mathf.Clamp(
            targetPosition.x,
            minX,
            maxX
        );

        targetPosition.z = Mathf.Clamp(
            targetPosition.z,
            minZ,
            maxZ
        );

        transform.position = targetPosition;
    }

    private void HandleMouseZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) < 0.01f)
            return;

        Zoom(scroll);
    }

    private void HandleTouchZoom()
    {
        if (state == CameraState.Moving)
        {
            if (Input.touchCount == 0)
            {
                state = CameraState.Free;
            }

            return;
        }

        if (Input.touchCount != 2)
        {
            if (Input.touchCount == 0)
            {
                state = CameraState.Free;
            }

            return;
        }

        if (state == CameraState.Free)
        {
            state = CameraState.Zooming;
        }

        if (state != CameraState.Zooming)
            return;

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        Vector2 previousPos0 = touch0.position - touch0.deltaPosition;
        Vector2 previousPos1 = touch1.position - touch1.deltaPosition;

        float previousDistance = Vector2.Distance(
            previousPos0,
            previousPos1
        );

        float currentDistance = Vector2.Distance(
            touch0.position,
            touch1.position
        );

        float delta = currentDistance - previousDistance;

        Zoom(delta);

        if (Input.touchCount == 0)
        {
            state = CameraState.Free;
        }
    }

    private void Zoom(float amount)
    {
        float newZoom = transform.position.y - amount * zoomSpeed;

        newZoom = Mathf.Clamp(
            newZoom,
            minZoom,
            maxZoom
        );

        Vector3 position = transform.position;
        position.y = newZoom;

        transform.position = position;
    }
}
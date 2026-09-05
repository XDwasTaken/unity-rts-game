using UnityEngine;

namespace RTS.Camera
{
    /// <summary>
    /// RTS camera controller - handles panning, zooming, and rotating
    /// </summary>
    public class RTSCamera : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float panSpeed = 20f;
        [SerializeField] private float rotateSpeed = 100f;

        [Header("Zoom")]
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minZoom = 10f;
        [SerializeField] private float maxZoom = 100f;

        [Header("Bounds")]
        [SerializeField] private Vector2 mapMin = Vector2.zero;
        [SerializeField] private Vector2 mapMax = new Vector2(100, 100);

        [Header("Edge Panning")]
        [SerializeField] private float edgePanThreshold = 10f;
        [SerializeField] private bool enableEdgePanning = true;

        private UnityEngine.Camera mainCamera;

        private void Start()
        {
            mainCamera = GetComponent<UnityEngine.Camera>();
            if (mainCamera == null)
            {
                mainCamera = UnityEngine.Camera.main;
            }
        }

        private void Update()
        {
            HandleMovement();
            HandleZoom();
            HandleRotation();
            ClampCameraPosition();
        }

        private void HandleMovement()
        {
            Vector3 direction = Vector3.zero;

            // Keyboard input (WASD)
            if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
            if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
            if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
            if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

            // Edge panning
            if (enableEdgePanning)
            {
                Vector3 mousePos = Input.mousePosition;
                if (mousePos.x < edgePanThreshold) direction += Vector3.left;
                if (mousePos.x > Screen.width - edgePanThreshold) direction += Vector3.right;
                if (mousePos.y < edgePanThreshold) direction += Vector3.back;
                if (mousePos.y > Screen.height - edgePanThreshold) direction += Vector3.forward;
            }

            if (direction != Vector3.zero)
            {
                transform.Translate(direction.normalized * panSpeed * Time.deltaTime, Space.Self);
            }
        }

        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                Vector3 pos = transform.position;
                pos.y -= scroll * zoomSpeed;
                pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);
                transform.position = pos;
            }
        }

        private void HandleRotation()
        {
            // Q and E keys for rotation (optional)
            if (Input.GetKey(KeyCode.Q))
            {
                transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.RotateAround(transform.position, Vector3.up, -rotateSpeed * Time.deltaTime);
            }
        }

        private void ClampCameraPosition()
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, mapMin.x, mapMax.x);
            pos.z = Mathf.Clamp(pos.z, mapMin.y, mapMax.y);
            transform.position = pos;
        }

        public void SetMapBounds(Vector2 min, Vector2 max)
        {
            mapMin = min;
            mapMax = max;
        }

        public void SetZoom(float zoom)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Clamp(zoom, minZoom, maxZoom);
            transform.position = pos;
        }
    }
}

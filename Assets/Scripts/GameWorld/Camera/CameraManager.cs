using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.GameWorld.CameraManager
{
    public class CameraManager : MonoBehaviour
    {
        Transform cameraTransform;
        Camera camera;

        // In secondds
        public static float ROTATION_TIME = 0.2f;
        public static float MOVEMENT_TIME = 0.5f;

        public delegate void CameraKeyHandler();
        Vector3 targetPosition = Vector3.zero;
        Vector2 movementVelocity = Vector2.zero;
        Vector2 movementSign = Vector2.zero;

        public const float MOVEMENT_ACCELERATION = 0.025f;
        public const float MAX_VELOCITY = 0.25f;
        public const float DRAG = 0.025f;

        public float zoomStartTime = 0;
        public float zoomCurrentTime = 1f;
        public float startZoom;
        public float targetZoom;
        public const float ZOOM_SPEED = 100f;
        public float scrollDirection;

        public bool isPanning = false;
        public Vector2 panVelocity = Vector2.zero;
        public Vector2 panStartPosition = Vector2.zero;
        public Vector2 panStartMousePosition = Vector2.zero;
        public Vector2 panCurrentPosition = Vector2.zero;
        public Vector2 panCurrentMousePosition = Vector2.zero;

        const float PAN_SPEED = 0.005f;
        const float PAN_LIMIT = 10f;

        void Awake()
        {
            cameraTransform = transform.Find("Main Camera");
            camera = cameraTransform.GetComponent<Camera>();
            startZoom = camera.orthographicSize;
            targetZoom = camera.orthographicSize;

            targetPosition = transform.position;
        }

        void Update()
        {
            HandleInput();
            // This doesn't work if it's in FixedUpdate
            ZoomCamera();
        }

        void FixedUpdate()
        {
            MoveCamera();
            PanCamera();
        }

        void HandleInput()
        {
            if (Input.GetKeyDown("w"))
            {
                OnMoveUpPressed();
            }

            if (Input.GetKeyUp("w"))
            {
                OnMoveUpReleased();
            }

            if (Input.GetKeyDown("s"))
            {
                OnMoveDownPressed();
            }

            if (Input.GetKeyUp("s"))
            {
                OnMoveDownReleased();
            }

            if (Input.GetKeyDown("a"))
            {
                OnMoveLeftPressed();
            }

            if (Input.GetKeyUp("a"))
            {
                OnMoveLeftReleased();
            }

            if (Input.GetKeyDown("d"))
            {
                OnMoveRightPressed();
            }

            if (Input.GetKeyUp("d"))
            {
                OnMoveRightReleased();
            }

            // Scrolling
            scrollDirection = Input.GetAxis("Mouse ScrollWheel");

            // Middle mouse pan
            if (Input.GetMouseButtonDown(2))
            {
                if (!isPanning)
                {
                    // start panning
                    isPanning = true;
                    panStartPosition = new Vector2(transform.position.x, transform.position.y);
                    panStartMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
            }

            if (Input.GetMouseButtonUp(2))
            {
                if (isPanning)
                {
                    isPanning = false;
                }
            }

            if (isPanning)
            {
                panCurrentPosition = new Vector2(transform.position.x, transform.position.y);
                panCurrentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        void OnMoveUpPressed()
        {
            movementSign.y = 1f;
        }

        void OnMoveUpReleased()
        {
            movementSign.y = 0f;
        }

        void OnMoveDownPressed()
        {
            movementSign.y = -1f;
        }

        void OnMoveDownReleased()
        {
            movementSign.y = 0f;
        }

        void OnMoveLeftPressed()
        {
            movementSign.x = -1f;
        }

        void OnMoveLeftReleased()
        {
            movementSign.x = 0;
        }

        void OnMoveRightPressed()
        {
            movementSign.x = 1f;
        }

        void OnMoveRightReleased()
        {
            movementSign.x = 0f;
        }

        void MoveCamera()
        {
            // x movement
            if (movementSign.x == 0)
            {
                if (movementVelocity.x > 0)
                {
                    movementVelocity.x -= DRAG;
                }
                else if (movementVelocity.x < 0)
                {
                    movementVelocity.x += DRAG;
                }

                if (Mathf.Abs(movementVelocity.x) < DRAG)
                {
                    movementVelocity.x = 0;
                }
            }
            else
            {
                movementVelocity.x += (MOVEMENT_ACCELERATION * movementSign.x);

                if (movementVelocity.x > 0)
                {
                    movementVelocity.x = Mathf.Min(movementVelocity.x, MAX_VELOCITY);
                }
                else if (movementVelocity.x < 0)
                {
                    movementVelocity.x = Mathf.Max(movementVelocity.x, -MAX_VELOCITY);
                }
            }

            // y movement
            if (movementSign.y == 0)
            {
                if (movementVelocity.y > 0)
                {
                    movementVelocity.y -= DRAG;
                }
                else if (movementVelocity.y < 0)
                {
                    movementVelocity.y += DRAG;
                }

                if (Mathf.Abs(movementVelocity.y) < DRAG)
                {
                    movementVelocity.y = 0;
                }
            }
            else
            {
                movementVelocity.y += (MOVEMENT_ACCELERATION * movementSign.y);

                if (movementVelocity.y > 0)
                {
                    movementVelocity.y = Mathf.Min(movementVelocity.y, MAX_VELOCITY);
                }
                else if (movementVelocity.y < 0)
                {
                    movementVelocity.y = Mathf.Max(movementVelocity.y, -MAX_VELOCITY);
                }
            }

            transform.position = new Vector3(
                transform.position.x + movementVelocity.x,
                transform.position.y + movementVelocity.y,
                transform.position.z
            );
        }

        void ZoomCamera()
        {
            if (scrollDirection != 0)
            {
                startZoom = camera.orthographicSize;
                // - instead of +: these 2 fields interact in the opposite way you'd expect
                targetZoom = camera.orthographicSize - Input.mouseScrollDelta.y;
                zoomStartTime = Time.time;
                zoomCurrentTime = 0;
            }

            if (zoomCurrentTime < 1f)
            {
                camera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, zoomCurrentTime * ZOOM_SPEED);
                zoomCurrentTime += Time.deltaTime;
            }
        }

        // TODO 
        // 1) take camera zoom into account: the further it's zoomed out the more movement is needed
        // 2) zoom towards where the cursor is pointing
        void PanCamera()
        {
            if (!isPanning) return;


            Vector2 mouseDifference = new Vector2(
                Mathf.Clamp(-(panCurrentMousePosition.x - panStartMousePosition.x) * PAN_SPEED, -PAN_LIMIT, PAN_LIMIT),
                Mathf.Clamp(-(panCurrentMousePosition.y - panStartMousePosition.y) * PAN_SPEED, -PAN_LIMIT, PAN_LIMIT)
            );

            Vector2 targetPosition = new Vector3(
                panStartPosition.x + mouseDifference.x,
                panStartPosition.y + mouseDifference.y
            );

            transform.position = new Vector3(
                targetPosition.x,
                targetPosition.y,
                transform.position.z
            );
        }
    }
}

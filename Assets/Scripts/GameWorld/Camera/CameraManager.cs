using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.GameWorld.Camera
{
    public class CameraManager : MonoBehaviour
    {
        Transform cameraTransform;

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

        void Awake()
        {
            cameraTransform = transform.Find("Main Camera");

            targetPosition = transform.position;
        }

        void Update()
        {
            HandleInput();
        }

        void FixedUpdate()
        {
            MoveCamera();
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

            if (Input.GetMouseButtonDown(2))
            {
            }

            if (Input.GetMouseButtonUp(2))
            {
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform cameraTransform;

    // In secondds
    public static float ROTATION_TIME = 0.2f;
    public static float MOVEMENT_TIME = 0.5f;

    public delegate void CameraKeyHandler();

    // TODO - put this somewhere more generic
    // public struct KeyBinding
    // {
    //     public string key;
    //     public CameraKeyHandler Handler;
    // }

    // public KeyBinding[] cameraKeyBindings;

    void Awake()
    {
        cameraTransform = transform.Find("Main Camera");

        // cameraKeyBindings = new KeyBinding[]
        // {
        //     new KeyBinding() {
        //         key = "e",
        //         Handler = OnRotateRightPressed
        //     },
        //     new KeyBinding() {
        //         key = "q",
        //         Handler = OnRotateLeftPressed
        //     },
        // };
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            OnRotateRightPressed();
        }

        if (Input.GetKeyDown("q"))
        {
            OnRotateLeftPressed();
        }

        if (Input.GetKeyDown("w"))
        {
            // OnMoveForwardPressed();
        }

        if (Input.GetKeyDown("a"))
        {
            // OnMoveForwardPressed();
        }

        if (Input.GetKeyDown("s"))
        {
            // OnMoveBackwardPressed();
        }

        if (Input.GetKeyDown("d"))
        {
            // OnMoveForwardPressed();
        }
    }


    void OnRotateLeftPressed()
    {
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y - 90,
            transform.rotation.z
        );
        StartCoroutine(RotateCameraTo(targetRotation));
    }

    void OnRotateRightPressed()
    {
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y + 90,
            transform.rotation.z
        );
        StartCoroutine(RotateCameraTo(targetRotation));
    }

    void OnMoveForwardPressed()
    {
        Vector3 targetPosition = transform.position + new Vector3(-1, 0, 1);
        StartCoroutine(MoveCameraTo(targetPosition));
    }

    void OnMoveBackwardPressed()
    {
        Vector3 targetPosition = transform.position + new Vector3(1, 0, -1);
        StartCoroutine(MoveCameraTo(targetPosition));
    }

    void OnMoveLeftPressed() { }

    void OnMoveRightPressed() { }

    private IEnumerator RotateCameraTo(Quaternion targetRotation)
    {
        float elapsed = 0;

        Quaternion startingRotation = transform.rotation;

        while (elapsed < ROTATION_TIME)
        {
            transform.rotation = Quaternion.Lerp(startingRotation, targetRotation, (elapsed / ROTATION_TIME));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private IEnumerator MoveCameraTo(Vector3 targetPosition)
    {
        float elapsed = 0;
        Debug.Log("moving to:");
        Debug.Log(targetPosition);

        Vector3 startingPosition = transform.position;

        while (elapsed < MOVEMENT_TIME)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, (elapsed / ROTATION_TIME));
            elapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("done");
        transform.position = targetPosition;
    }
}

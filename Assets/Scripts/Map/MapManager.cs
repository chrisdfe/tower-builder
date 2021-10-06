using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;

namespace TowerBuilder.UI
{
    public class MapManager : MonoBehaviour
    {
        Plane plane;
        Vector3 worldPosition;
        GameObject cubeCursor;

        void Awake()
        {
            // Vector3 cameraAngle = Camera.main.transform.rotation.eulerAngles;
            plane = new Plane(Vector3.forward, 0);
        }

        void Update()
        {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance);
            }
            Debug.Log("worldPosition: " + worldPosition.y + ", " + worldPosition.x);
        }
    }
}
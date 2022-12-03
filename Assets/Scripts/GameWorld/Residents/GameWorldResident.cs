using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Behaviors;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Residents
{
    public class GameWorldResident : MonoBehaviour
    {
        public Resident resident;
        public ResidentBehavior residentBehavior;

        public CurrentAndNext<(TimeValue, CellCoordinates)> currentAndNextPosition;

        Transform cube;
        Color defaultColor;

        public enum ColorKey
        {
            Base,
            Default,
            Hover,
            Inspected,
            Destroy,
            ValidBlueprint,
            InvalidBlueprint
        }

        public static Dictionary<ColorKey, Color> ColorMap = new Dictionary<ColorKey, Color>() {
            { ColorKey.Base, Color.grey },
            { ColorKey.Hover, Color.green },
            { ColorKey.Inspected, Color.cyan },
            { ColorKey.Destroy, Color.red },
            { ColorKey.ValidBlueprint, Color.blue },
            { ColorKey.InvalidBlueprint, Color.red },
        };

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            cube = transform.Find("Cube");
            defaultColor = cube.GetComponent<MeshRenderer>().material.color;
        }

        void OnDestroy()
        {
            Teardown();
        }

        void Update()
        {
            UpdateMovement();
        }

        public void Setup()
        {
            UpdatePosition();
        }

        public void Teardown() { }

        /* 
            Public Interface
        */
        public void UpdatePosition()
        {
            transform.position = GameWorldUtils.CellCoordinatesToPosition(resident.cellCoordinates);
        }

        public void SetColor(ColorKey key)
        {
            if (key == ColorKey.Default)
            {
                SetColor(defaultColor, 1f);
            }
            else
            {
                Color color = ColorMap[key];

                if (color != null)
                {
                    SetColor(color, 1f);
                }
            }
        }

        /* 
            Internals
        */
        void UpdateMovement()
        {
            if (currentAndNextPosition == null) return;
            var ((startTick, startCoordinates), (endTick, endCoordinates)) = currentAndNextPosition;
            float normalizedTickProgress = GameWorldTimeSystemManager.Find().normalizedTickProgress;

            transform.position = Vector3.Lerp(
                GameWorldUtils.CellCoordinatesToPosition(startCoordinates),
                GameWorldUtils.CellCoordinatesToPosition(endCoordinates),
                normalizedTickProgress
            );
        }

        void SetColor(Color color, float alpha = 1.0f)
        {
            Material material = cube.GetComponent<MeshRenderer>().material;
            material.color = new Color(color.r, color.g, color.b, alpha);
        }

        /* 
            Static API
         */
        public static GameWorldResident Create(Transform parent)
        {
            GameWorldResidentsManager residentsManager = GameWorldResidentsManager.Find();
            GameObject prefab = residentsManager.assetList.FindByKey(GameWorldResidentsManager.AssetKey.Resident);
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;

            GameWorldResident gameWorldResident = gameObject.GetComponent<GameWorldResident>();
            return gameWorldResident;
        }
    }
}

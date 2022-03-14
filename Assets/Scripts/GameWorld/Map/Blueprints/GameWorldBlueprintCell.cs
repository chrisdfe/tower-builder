using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Blueprints
{
    public class GameWorldBlueprintCell : MonoBehaviour
    {
        static Color VALID_COLOR = new Color(0, 0, 255, 0.5f);
        static Color INVALID_COLOR = new Color(255, 0, 0, 0.5f);

        BlueprintCell blueprintCell;

        Transform cellCube;

        GameObject roomEntrancePrefab;

        List<GameObject> roomEntranceGameObjects = new List<GameObject>();

        GameWorldBlueprint parentBlueprint;

        public void SetParentBlueprint(GameWorldBlueprint parentBlueprint)
        {
            this.parentBlueprint = parentBlueprint;
        }

        public void SetRoomBlueprintCell(BlueprintCell blueprintCell)
        {
            this.blueprintCell = blueprintCell;
            UpdatePosition();
            UpdateMaterialColor();
        }

        public void UpdatePosition()
        {
            transform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(blueprintCell.roomCell.coordinates);
        }

        public void Initialize()
        {
            InitializeRoomEntrances();
        }

        public void UpdateMaterialColor()
        {
            if (IsValid())
            {
                cellCube.GetComponent<Renderer>().material.color = VALID_COLOR;
            }
            else
            {
                cellCube.GetComponent<Renderer>().material.color = INVALID_COLOR;
            }
        }

        void Awake()
        {
            roomEntrancePrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomEntrance");
            cellCube = transform.Find("CellCube");
        }

        void OnDestroy()
        {
            foreach (GameObject roomEntranceGameObject in roomEntranceGameObjects)
            {
                Destroy(roomEntranceGameObject);
            }
        }

        void InitializeRoomEntrances()
        {
            List<RoomEntrance> roomEntrances = blueprintCell.roomCell.entrances;
            float TILE_SIZE = TowerBuilder.Stores.Map.Rooms.Constants.TILE_SIZE;

            foreach (RoomEntrance roomEntrance in roomEntrances)
            {
                // TODO - all of this stuff should probably go in GameRoomEntrance
                GameObject entranceGameObject = Instantiate(roomEntrancePrefab);
                entranceGameObject.transform.parent = transform;
                if (roomEntrance.position == RoomEntrancePosition.Left)
                {
                    // RoomEntrancePosition.Right
                    entranceGameObject.transform.localPosition = new Vector3(
                        -(TILE_SIZE / 2) + (entranceGameObject.transform.localScale.x / 2),
                        -(TILE_SIZE / 2) + (entranceGameObject.transform.localScale.y / 2),
                        0
                    );
                }
                else
                {
                    // RoomEntrancePosition.Right
                    entranceGameObject.transform.localPosition = new Vector3(
                        (TILE_SIZE / 2) - (entranceGameObject.transform.localScale.x / 2),
                        -(TILE_SIZE / 2) + (entranceGameObject.transform.localScale.y / 2),
                        0
                    );
                }

                roomEntranceGameObjects.Add(entranceGameObject);
            }
        }

        bool IsValid()
        {
            if (!blueprintCell.parentBlueprint.IsValid())
            {
                return false;
            }

            if (!blueprintCell.IsValid())
            {
                return false;
            }

            return true;
        }
    }
}

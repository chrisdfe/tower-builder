using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
    public class GameRoomBlueprintCell : MonoBehaviour
    {
        static Color VALID_COLOR = new Color(0, 0, 255, 0.5f);
        static Color INVALID_COLOR = new Color(255, 0, 0, 0.5f);

        // public MapRoomBlueprint parentMapRoomBlueprint;

        BlueprintCell roomBlueprintCell;

        Renderer _renderer;

        GameObject mapRoomEntrancePrefab;

        List<GameObject> roomEntranceGameObjects = new List<GameObject>();

        GameRoomBlueprint parentBlueprint;

        public void SetParentBlueprint(GameRoomBlueprint parentBlueprint)
        {
            this.parentBlueprint = parentBlueprint;
        }

        public void SetRoomBlueprintCell(BlueprintCell roomBlueprintCell)
        {
            this.roomBlueprintCell = roomBlueprintCell;
            transform.localPosition = GameMapCellHelpers.CellCoordinatesToPosition(roomBlueprintCell.cellCoordinates);
            UpdateMaterialColor();


        }

        public void Initialize()
        {
            List<RoomEntrance> roomEntrances = roomBlueprintCell.GetRoomEntrances();
            float TILE_SIZE = TowerBuilder.Stores.Map.Rooms.Constants.TILE_SIZE;

            // Debug.Log(roomBlueprintCell.GetRelativeCellCoordinates());
            if (roomEntrances.Count > 0)
            {
                foreach (RoomEntrance roomEntrance in roomEntrances)
                {
                    GameObject entranceGameObject = Instantiate(mapRoomEntrancePrefab);
                    // entranceGameObject.transform.parent = parentBlueprint.transform;
                    entranceGameObject.transform.parent = transform;
                    if (roomEntrance.position == RoomEntrancePosition.Left)
                    {
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
                    // TODO - position it
                    roomEntranceGameObjects.Add(entranceGameObject);
                }
            }
        }

        public void UpdateMaterialColor()
        {
            if (IsValid())
            {
                _renderer.material.color = VALID_COLOR;
            }
            else
            {
                _renderer.material.color = INVALID_COLOR;
            }
        }

        void Awake()
        {
            mapRoomEntrancePrefab = Resources.Load<GameObject>("Prefabs/Map/GameRoomEntrance");
            _renderer = GetComponent<Renderer>();
        }

        void OnDestroy()
        {
            // TODO - do i need to od this?
            foreach (GameObject roomEntranceGameObject in roomEntranceGameObjects)
            {
                Destroy(roomEntranceGameObject);
            }
        }


        bool IsValid()
        {
            if (!roomBlueprintCell.parentBlueprint.IsValid())
            {
                return false;
            }

            if (!roomBlueprintCell.IsValid())
            {
                return false;
            }

            return true;
        }

    }
}

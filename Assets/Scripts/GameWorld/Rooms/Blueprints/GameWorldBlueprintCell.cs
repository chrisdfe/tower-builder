using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Blueprints;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms.Blueprints
{
    public class GameWorldBlueprintCell : MonoBehaviour
    {
        static Color VALID_COLOR = new Color(0, 0, 255, 0.5f);
        static Color INVALID_COLOR = new Color(255, 0, 0, 0.5f);

        public GameWorldBlueprint parentBlueprint;
        public BlueprintCell blueprintCell;
        public GameWorldRoomCell gameWorldRoomCell;

        public void Initialize()
        {
            SetColor();
        }

        void SetColor()
        {
            if (IsValid())
            {
                gameWorldRoomCell.SetColor(VALID_COLOR);
            }
            else
            {
                gameWorldRoomCell.SetColor(INVALID_COLOR);
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

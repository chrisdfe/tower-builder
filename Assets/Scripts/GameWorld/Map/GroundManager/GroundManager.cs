using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.ApplicationState.UI;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map
{
    public class GroundManager : MonoBehaviour
    {
        GameObject groundPlaceholder;

        // TODO - put these somewhere else
        const int MAP_CELLS_WIDTH = 50;
        const int MAP_CELLS_HEIGHT = 50;
        const int GROUND_STARTING_FLOOR = -1;

        void Awake() { }

        void Start() { }

        void OnDestroy() { }
    }
}
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Residents
{
    public class GameWorldResident : GameWorldEntity
    {
        public CurrentAndNext<(TimeValue, CellCoordinates)> currentAndNextPosition;

        public void OnTick()
        {

        }

        /* 
            Internals
        */
        /*
        void UpdateMovement()
        {
            if (currentAndNextPosition != null)
            {
                var ((startTick, startCoordinates), (endTick, endCoordinates)) = currentAndNextPosition;
                float normalizedTickProgress = GameWorldTimeSystemManager.Find().normalizedTickProgress;
                // Debug.Log($"currentA/ndNextPosition: {startCoordinates}, {endCoordinates}, {normalizedTickProgress}");

                // TODO - these coordinates need to be relative not absolute now
                entityMeshWrapper.positionOffset = Vector3.Lerp(
                    GameWorldUtils.CellCoordinatesToPosition(startCoordinates),
                    GameWorldUtils.CellCoordinatesToPosition(endCoordinates),
                    normalizedTickProgress
                );
            }
        }
        */
    }
}

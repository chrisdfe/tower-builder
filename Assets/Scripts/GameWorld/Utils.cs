using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public static class GameWorldUtils
    {
        public static CurrentAndNext<float> GetNormalizedCurrentAndNextTickTimesBetween(TimeValue fromTime, TimeValue toTime)
        {
            float normalizedCurrentTickTime = MathUtils.NormalizeFloat(
                Registry.appState.Time.time.ToRelative().AsMinutes(),
                fromTime.AsMinutes(),
                toTime.AsMinutes()
            );

            float normalizedNextTickTime = MathUtils.NormalizeFloat(
                Registry.appState.Time.queries.nextTickTimeValue.ToRelative().AsMinutes(),
                fromTime.AsMinutes(),
                toTime.AsMinutes()
            );

            return new CurrentAndNext<float>()
            {
                current = normalizedCurrentTickTime,
                next = normalizedNextTickTime
            };
        }

        public static int RoundToNearestTile(float number)
        {
            float TILE_SIZE = DataTypes.Entities.Rooms.Constants.TILE_SIZE;
            float rounded = (float)Math.Round(number / TILE_SIZE) * TILE_SIZE;
            return (int)rounded;
        }

        public static Vector3 CellCoordinatesToPosition(CellCoordinates cellCoordinates, float zIndex = 0f)
        {
            return new Vector3(
                RoundToNearestTile(cellCoordinates.x),
                RoundToNearestTile(cellCoordinates.floor),
                zIndex
            );
        }
    }
}
using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
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
                Registry.appState.Time.nextTickTimeValue.ToRelative().AsMinutes(),
                fromTime.AsMinutes(),
                toTime.AsMinutes()
            );

            return new CurrentAndNext<float>()
            {
                current = normalizedCurrentTickTime,
                next = normalizedNextTickTime
            };
        }

        public static CellCoordinates RoundVector2ToCellCoordinates(Vector2 vector)
        {
            return new CellCoordinates(
                RoundToNearestInt(vector.x, Entities.Constants.CELL_WIDTH),
                RoundToNearestInt(vector.y, Entities.Constants.CELL_HEIGHT)
            );
        }

        public static Vector3 CellCoordinatesToPosition(CellCoordinates cellCoordinates, float zIndex = 0f)
        {
            return new Vector3(
                cellCoordinates.x * Entities.Constants.CELL_WIDTH,
                cellCoordinates.floor * Entities.Constants.CELL_HEIGHT,
                zIndex
            );
        }

        static int RoundToNearestInt(float number, float nearest)
        {
            float rounded = (float)Math.Round(number / nearest) * nearest;
            return (int)rounded;
        }
    }
}
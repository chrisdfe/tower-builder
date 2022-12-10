using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
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

        public static CellCoordinates RoundVector2ToCellCoordinates(Vector2 vector)
        {
            Vector2 TileSize = DataTypes.Entities.Rooms.Constants.TILE_SIZE;

            return new CellCoordinates(
                RoundToNearestInt(vector.x, TileSize.x),
                RoundToNearestInt(vector.y, TileSize.y)
            );
        }

        public static Vector3 CellCoordinatesToPosition(CellCoordinates cellCoordinates, float zIndex = 0f)
        {
            Vector2 TileSize = DataTypes.Entities.Rooms.Constants.TILE_SIZE;

            return new Vector3(
                cellCoordinates.x * TileSize.x,
                cellCoordinates.floor * TileSize.y,
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
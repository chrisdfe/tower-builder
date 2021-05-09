using System;
using System.Linq;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Map
{
    //
    // Helpers that don't really fit into any other category
    //
    public static class MapMiscHelpers
    {
        public static List<MapCoordinates> overlayRoomShape(
          MapCoordinates overlayCoordinates,
          List<MapCoordinates> roomCells
        )
        {
            return roomCells.Select((MapCoordinates roomCellCoordinates) =>
            {
                return new MapCoordinates()
                {
                    x = roomCellCoordinates.x + overlayCoordinates.x,
                    y = roomCellCoordinates.y + overlayCoordinates.y,
                };
            }).ToList();
        }
    }
}
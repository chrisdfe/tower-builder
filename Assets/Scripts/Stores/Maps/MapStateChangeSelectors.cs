using System;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Map
{
    public partial class MapStore
    {
        public static class StateChangeSelectors
        {
            public static List<MapRoom> GetAddedRooms(MapState previousState, MapState state)
            {
                List<MapRoom> result = new List<MapRoom>();

                List<MapRoom> previousMapRooms = previousState.mapRooms;
                List<MapRoom> mapRooms = state.mapRooms;

                foreach (MapRoom mapRoom in mapRooms)
                {
                    if (!previousMapRooms.Contains(mapRoom))
                    {
                        result.Add(mapRoom);
                    }
                }

                // foreach(MapRoom)
                return result;
            }
        }
    }
}
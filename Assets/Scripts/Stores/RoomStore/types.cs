using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores
{
    public enum RoomType
    {
        CommonArea,
        Residence,
        Workplace,
        Transportation,
        Park,
    }

    public enum RoomKey
    {
        EmptyFloor,
        Lobby,
        Office,
        Condo,
        Elevator,
        Stairwell,
        SmallPark,
    }

    public struct RoomState
    {
        public Dictionary<string, RoomKey> roomKeyMap;
    }
}
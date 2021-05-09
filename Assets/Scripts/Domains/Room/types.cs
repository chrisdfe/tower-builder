using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Rooms
{
    public class RoomKeyMap : Dictionary<string, RoomKey> { };

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

    public enum RoomType
    {
        CommonArea,
        Residence,
        Workplace,
        Transportation,
        Park,
    }

    public struct RoomDetails
    {
        public string title;
        public int price;
        public RoomType[] types;
    }

    public class RoomDetailsMap : Dictionary<RoomKey, RoomDetails> { }

    public struct RoomState
    {
        public RoomKeyMap roomKeyMap;
    }
}
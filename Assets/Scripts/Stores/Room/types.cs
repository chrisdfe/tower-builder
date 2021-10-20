using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Rooms
{
    public enum RoomKey
    {
        None,
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
        None,
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
}
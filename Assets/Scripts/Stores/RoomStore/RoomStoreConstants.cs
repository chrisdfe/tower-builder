using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores
{
    public static class RoomStoreConstants
    {
        public static RoomDetailsMap ROOM_DETAILS_MAP = new RoomDetailsMap()
        {
            [RoomKey.EmptyFloor] = new RoomDetails()
            {
                title = "Lobby",
                price = 1000,
                types = new RoomType[] { RoomType.CommonArea },
            },
            [RoomKey.Lobby] = new RoomDetails()
            {
                title = "Lobby",
                price = 5000,
                types = new RoomType[] { RoomType.CommonArea },
            },
            [RoomKey.Office] = new RoomDetails()
            {
                title = "Office",
                price = 20000,
                types = new RoomType[] { RoomType.Workplace },
            },
            [RoomKey.Condo] = new RoomDetails()
            {
                title = "Condo",
                price = 50000,
                types = new RoomType[] { RoomType.Residence },
            },
            [RoomKey.Elevator] = new RoomDetails()
            {
                title = "Elevator",
                price = 2000,
                types = new RoomType[] { RoomType.Transportation },
            },
            [RoomKey.Stairwell] = new RoomDetails()
            {
                title = "Stairwell",
                price = 5000,
                types = new RoomType[] { RoomType.Transportation },
            },
            [RoomKey.SmallPark] = new RoomDetails()
            {
                title = "Small Park",
                price = 10000,
                types = new RoomType[] { RoomType.CommonArea, RoomType.Park },
            },
        };
    }
}
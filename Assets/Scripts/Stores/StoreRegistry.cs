using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores
{
    public class StoreRegistry
    {
        public RoomStore roomStore = new RoomStore();
    }

    public static class Registry
    {
        public static StoreRegistry storeRegistry = new StoreRegistry();
    }
}

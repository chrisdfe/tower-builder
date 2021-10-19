using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using TowerBuilder.Stores.Notifications;
using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Time;
using TowerBuilder.Stores.Wallet;

namespace TowerBuilder.Stores
{
    public class StoreBase<StateType>
    {
        public StateType state;
    }
}

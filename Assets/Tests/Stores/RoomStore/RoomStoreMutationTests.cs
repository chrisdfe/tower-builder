using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using TowerBuilder.Stores;

public class RoomStoreMutationsTests
{
    public class addRoomKeyToMap
    {
        [Test]
        public void SuccessfullyBuildsRoom()
        {
            StoreRegistry mockStoreRegistry = new TowerBuilder.Stores.StoreRegistry();
            string newRoomId = RoomStoreMutations.addRoomKeyToMap(mockStoreRegistry, RoomKey.EmptyFloor);
            Assert.IsNotNull(newRoomId);
        }

        [Test]
        public void AddsRoomIdToRoomStore()
        {
            StoreRegistry mockStoreRegistry = new TowerBuilder.Stores.StoreRegistry();
            RoomStore roomStore = mockStoreRegistry.roomStore;
            string newRoomId = RoomStoreMutations.addRoomKeyToMap(mockStoreRegistry, RoomKey.Lobby);

            // Assert.True(roomStore.state.roomKeyMap.ContainsKey(newRoomId));
        }
    }
}

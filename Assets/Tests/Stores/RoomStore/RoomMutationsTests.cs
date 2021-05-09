using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using TowerBuilder.Domains;
using TowerBuilder.Domains.Rooms;

public class RoomMutationsTests
{
    public class addRoomKeyToMap
    {
        [Test]
        public void ItSuccessfullyBuildsRoom()
        {
            StoreRegistry mockStoreRegistry = new StoreRegistry();
            string newRoomId = RoomMutations.addRoomKeyToMap(mockStoreRegistry, RoomKey.EmptyFloor);
            Assert.IsNotNull(newRoomId);
        }

        [Test]
        public void ItAddsRoomIdToRoomStore()
        {
            StoreRegistry mockStoreRegistry = new StoreRegistry();
            RoomStore roomStore = mockStoreRegistry.roomStore;
            string newRoomId = RoomMutations.addRoomKeyToMap(mockStoreRegistry, RoomKey.Lobby);

            Assert.True(roomStore.state.roomKeyMap.ContainsKey(newRoomId));
        }

        // [Test]
        // public void ItFiresRoomStateUpdatedEvent()
        // {
        // }
    }
}

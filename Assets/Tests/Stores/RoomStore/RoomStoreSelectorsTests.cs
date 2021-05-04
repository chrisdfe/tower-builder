using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using TowerBuilder.Stores;

public class RoomStoreSelectorsTests
{
    public class findRoomKeyById
    {
        [Test]
        public void SuccessfullyReturnsRoomKey()
        {
            StoreRegistry mockStoreRegistry = new TowerBuilder.Stores.StoreRegistry()
            {
                roomStore = new RoomStore()
                {
                    state = new RoomState
                    {
                        roomKeyMap = new RoomKeyMap()
                        {
                            ["1"] = RoomKey.Lobby
                        }
                    }
                }
            };

            RoomKey roomKey = RoomStoreSelectors.findRoomKeyById(mockStoreRegistry, "1");
            Assert.AreEqual(roomKey, RoomKey.Lobby);
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;

// using TowerBuilder.Domains;
// using TowerBuilder.Domains.Rooms;

// public class RoomSelectorsTests
// {
//     public class findRoomKeyById
//     {
//         [Test]
//         public void SuccessfullyReturnsRoomKey()
//         {
//             StoreRegistry mockStoreRegistry = new StoreRegistry()
//             {
//                 roomStore = new RoomStore()
//                 {
//                     state = new RoomState
//                     {
//                         roomKeyMap = new RoomKeyMap()
//                         {
//                             ["1"] = RoomKey.Lobby
//                         }
//                     }
//                 }
//             };

//             RoomKey roomKey = RoomSelectors.findRoomKeyById(mockStoreRegistry, "1");
//             Assert.AreEqual(roomKey, RoomKey.Lobby);
//         }
//     }
// }

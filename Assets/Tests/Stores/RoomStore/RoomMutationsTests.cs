// using System.Collections;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;

// using TowerBuilder.Domains;
// using TowerBuilder.Domains.Rooms;

// public class RoomMutationsTests
// {
//     public class addRoomKeyToMap
//     {
//         [Test]
//         public void ItSuccessfullyBuildsRoom()
//         {
//             AppState mockAppState = new AppState();
//             string newRoomId = RoomMutations.addRoomKeyToMap(mockAppState, RoomKey.EmptyFloor);
//             Assert.IsNotNull(newRoomId);
//         }

//         [Test]
//         public void ItAddsRoomIdToRoomStore()
//         {
//             AppState mockAppState = new AppState();
//             RoomStore roomStore = mockAppState.roomStore;
//             string newRoomId = RoomMutations.addRoomKeyToMap(mockAppState, RoomKey.Lobby);

//             Assert.True(roomStore.state.roomKeyMap.ContainsKey(newRoomId));
//         }

//         // [Test]
//         // public void ItFiresRoomStateUpdatedEvent()
//         // {
//         // }
//     }
// }

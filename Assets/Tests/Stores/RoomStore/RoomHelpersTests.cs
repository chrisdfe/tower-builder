using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using TowerBuilder.Domains;
using TowerBuilder.Domains.Rooms;

public class RoomHelpersTests
{
    [Test]
    public void findRoomKeyById()
    {
        RoomKeyMap roomKeyMap = new RoomKeyMap()
        {
            ["1"] = RoomKey.Lobby
        };

        RoomKey result = RoomHelpers.findRoomKeyById("1", roomKeyMap);
        Assert.AreEqual(result, RoomKey.Lobby);
    }

    [Test]
    public void findRoomDetailsById()
    {
        string roomId = "1";
        RoomKey roomKey = RoomKey.Lobby;

        RoomKeyMap roomKeyMap = new RoomKeyMap()
        {
            [roomId] = roomKey
        };

        RoomDetails expectedRoomDetails = RoomConstants.ROOM_DETAILS_MAP[roomKey];

        RoomDetails result = RoomHelpers.findRoomDetailsById(roomId, roomKeyMap);
        Assert.AreEqual(result, expectedRoomDetails);
    }
}

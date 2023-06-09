using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class Room : EntityGroup
    {
        public Room() { }
        public Room(RoomDefinition roomDefinition) : base(roomDefinition)
        {

        }
    }
}



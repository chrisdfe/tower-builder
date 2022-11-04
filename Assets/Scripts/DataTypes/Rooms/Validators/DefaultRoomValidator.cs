using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public class DefaultRoomValidator : RoomValidatorBase
    {
        public DefaultRoomValidator(Room room) : base(room) { }
    }
}
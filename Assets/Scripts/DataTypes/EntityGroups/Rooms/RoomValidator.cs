using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Wheels;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomValidator : EntityGroupValidator
    {
        protected override List<RoomValidator.ValidationFunc> customValidators =>
            new List<RoomValidator.ValidationFunc>()
            {
                // GenericEntityValidations.CreateValidateEntityCellIsNotOnFloor(0),
                // ValidateRoomAboveOtherRoomOrWheels,
                // ValidateAcceptableOverhang,
            };


        public RoomValidator(Room room) : base(room) { }
    }
}
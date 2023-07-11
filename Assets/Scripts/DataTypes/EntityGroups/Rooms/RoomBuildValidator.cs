using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Wheels;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomBuildValidator : EntityGroupValidator
    {
        protected override List<EntityGroupValidator.ValidationFunc> customValidators =>
            new List<EntityGroupValidator.ValidationFunc>()
            {
                // GenericEntityValidations.CreateValidateEntityCellIsNotOnFloor(0),
                // ValidateRoomIsAboveOtherRoomOrWheels,
                // ValidateAcceptableOverhang,
            };


        public RoomBuildValidator(Room room) : base(room) { }
    }
}
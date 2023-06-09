using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomDefinitions : EntityGroupDefinitionList
    {
        public override ListWrapper<EntityGroupDefinition> Definitions =>
            new ListWrapper<EntityGroupDefinition>(
                new List<EntityGroupDefinition>() {
                    new OfficeRoomDefinition() {
                        key = "Office",
                        title = "Office",
                        category = "Office",
                    },

                    new RoomDefinition() {
                        key = "Lobby",
                        title = "Lobby",
                        category = "Lobby"
                    }
                }
            );
    }
}


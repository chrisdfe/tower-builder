using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomDefinitions : EntityGroupDefinitionList
    {
        public override ListWrapper<EntityGroupDefinition> Definitions =>
            new ListWrapper<EntityGroupDefinition>(
                new List<EntityGroupDefinition>() {
                    new RoomDefinition() {
                        key = "Office",
                        title = "Office",
                        category = "Office",

                        builderFactory = (EntityGroupDefinition definition) => new OfficeRoomBuilder(definition)
                    },

                    new RoomDefinition() {
                        key = "Lobby",
                        title = "Lobby",
                        category = "Lobby",

                        builderFactory = (EntityGroupDefinition definition) => new LobbyRoomBuilder(definition)
                    }
                }
            );
    }
}


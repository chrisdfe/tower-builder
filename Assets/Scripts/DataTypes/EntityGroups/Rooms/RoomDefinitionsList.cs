using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomDefinitions : EntityGroupDefinitionList
    {
        public override List<EntityGroupDefinition> Definitions =>
            new List<EntityGroupDefinition>(
                new List<EntityGroupDefinition>() {
                    new RoomDefinition() {
                        key = "Empty",
                        title = "Empty",
                        category = "Empty",

                        builderFactory = (EntityGroupDefinition definition) => new EmptyEntityGroupBuilder(definition)
                    },

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


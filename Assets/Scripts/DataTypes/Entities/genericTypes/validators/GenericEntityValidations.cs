using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public static class GenericEntityValidations
    {
        public static EntityValidationErrorList ValidateWalletHasEnoughMoney(AppState appState, Entity entity)
        {
            if (appState.Wallet.balance < entity.price)
            {
                return new EntityValidationErrorList("Insufficient funds.");
            }

            return new EntityValidationErrorList();
        }

        public static EntityValidationErrorList ValidateIsInsideRoom(AppState appState, Entity entity)
        {
            foreach (CellCoordinates cellCoordinates in entity.cellCoordinatesList.items)
            {
                Room entityRoom = appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates);

                if (entityRoom == null)
                {
                    return new EntityValidationErrorList($"{entity.definition.title} must be placed inside.");
                }
            }

            return new EntityValidationErrorList();
        }

        public static EntityValidationErrorList ValidateNotDirectlyNextToEntityOfSameType(AppState appState, Entity entity, CellCoordinates cellCoordinates)
        {
            // TODO - entity layers
            // TODO - check above + to the left + right and below + to the left and right (not directly above or below, that's ok)
            // Room leftRoom = allRooms.FindRoomAtCell(cellCoordinates.coordinatesLeft);
            // Room rightRoom = allRooms.FindRoomAtCell(cellCoordinates.coordinatesRight);

            // if (
            //     (leftRoom != null && leftRoom.definition.category == entity.definition.category) ||
            //     (rightRoom != null && rightRoom.definition.category == entity.definition.category)
            // )
            // {
            //     return new EntityValidationErrorList($"{entity.definition.title} cannot be placed directly next to each other.");
            // }

            return new EntityValidationErrorList();
        }

        public static EntityValidationErrorList ValidateEntityIsNotOverlappingAnotherEntity(AppState appState, Entity entity)
        {
            IEntityStateSlice stateSlice = appState.Entities.GetStateSlice(entity);

            Entity overlappingEntity = stateSlice.entityList.Find((otherEntity) =>
                otherEntity != entity &&
                otherEntity.cellCoordinatesList.OverlapsWith(entity.cellCoordinatesList)
            );

            if (overlappingEntity != null)
            {
                string entityLabel = Entity.TypeLabels.ValueFromKey(entity.type);
                return new EntityValidationErrorList($"You cannot build {entityLabel}s on top of each other.");
            }

            return new EntityValidationErrorList();
        }

        public static EntityValidationErrorList ValidateEntityIsNotUnderground(AppState appState, Entity entity)
        {
            foreach (int floor in entity.cellCoordinatesList.floorValues)
            {
                if (floor < 0)
                {
                    string entityLabel = Entity.TypeLabels.ValueFromKey(entity.type);
                    return new EntityValidationErrorList($"You cannot build {entityLabel} underground.");
                }
            }

            return new EntityValidationErrorList();
        }

        public static EntityValidationFunc CreateValidateEntityIsOnFloor(int floor) =>
            (AppState appState, Entity entity) =>
            {
                if (entity.cellCoordinatesList.lowestFloor != floor)
                {
                    string entityLabel = Entity.TypeLabels.ValueFromKey(entity.type);
                    return new EntityValidationErrorList($"{entityLabel} must be placed on floor {floor + 1}");
                }

                return new EntityValidationErrorList();
            };

        public static EntityValidationFunc CreateValidateEntityCellIsNotOnFloor(int floor) =>
            (AppState appState, Entity entity) =>
            {
                if (entity.cellCoordinatesList.lowestFloor == floor)
                {
                    string entityLabel = Entity.TypeLabels.ValueFromKey(entity.type);
                    return new EntityValidationErrorList($"{entityLabel} must not be placed on floor {floor + 1}");
                }

                return new EntityValidationErrorList();
            };
    }
}
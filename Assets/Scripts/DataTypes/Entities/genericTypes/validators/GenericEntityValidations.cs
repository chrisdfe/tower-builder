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
    }

    public static class GenericEntityCellValidations
    {
        public static EntityValidationErrorList ValidateEntityCellIsNotOverlappingAnotherEntity(AppState appState, Entity entity, CellCoordinates cellCoordinates)
        {
            IEntityStateSlice stateSlice = appState.Entities.GetStateSlice(entity);

            // TODO - entity layers
            Entity overlappingEntity = stateSlice.entityList.Find((otherEntity) =>
                otherEntity != entity &&
                otherEntity.cellCoordinatesList.Contains(cellCoordinates)
            );

            if (overlappingEntity != null)
            {
                string entityLabel = Entity.TypeLabels.ValueFromKey(entity.type);
                return new EntityValidationErrorList($"You cannot build {entityLabel}s on top of each other.");
            }

            return new EntityValidationErrorList();
        }

        public static EntityValidationErrorList ValidateEntityCellIsNotUnderground(AppState appState, Entity entity, CellCoordinates cellCoordinates)
        {
            if (cellCoordinates.floor < 0)
            {
                string entityLabel = Entity.TypeLabels.ValueFromKey(entity.type);
                return new EntityValidationErrorList($"You cannot build {entityLabel} underground.");
            }

            return new EntityValidationErrorList();
        }

        public static EntityCellValidationFunc CreateValidateEntityCellIsOnFloor(int floor) =>
            (AppState appState, Entity entity, CellCoordinates cellCoordinates) =>
            {
                // Since rooms can be multiple tiles high, make sure the cell we're validating here is the bottom-most cell
                bool isOnBottom = entity.cellCoordinatesList.asRelativeCoordinates.lowestFloor == 0;
                if (isOnBottom && cellCoordinates.floor != floor)
                {
                    return new EntityValidationErrorList($"{entity.definition.title} must be placed on floor {floor + 1}");
                }

                return new EntityValidationErrorList();
            };

        public static EntityCellValidationFunc CreateValidateEntityCellIsNotOnFloor(int floor) =>
            (AppState appState, Entity entity, CellCoordinates cellCoordinates) =>
            {
                // Since rooms can be multiple tiles high, make sure the cell we're validating here is the bottom-most cell
                bool isOnBottom = entity.cellCoordinatesList.asRelativeCoordinates.lowestFloor == 0;
                if (isOnBottom && cellCoordinates.floor == floor)
                {
                    return new EntityValidationErrorList($"{entity.definition.title} must not be placed on floor {floor + 1}");
                }

                return new EntityValidationErrorList();
            };
    }
}
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Entities;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Validators.Entities
{
    public static class GenericEntityValidations
    {
        public static ListWrapper<ValidationError> ValidateWalletHasEnoughMoney(AppState appState, Entity entity)
        {
            if (appState.Wallet.balance < entity.price)
            {
                return Validator.CreateSingleItemValidationErrorList("Insufficient funds.");
            }

            return new ListWrapper<ValidationError>();
        }

        public static ListWrapper<ValidationError> ValidateIsInsideRoom(AppState appState, Entity entity)
        {
            foreach (CellCoordinates cellCoordinates in entity.cellCoordinatesList.items)
            {
                Room entityRoom = appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates);

                if (entityRoom == null)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must be placed inside.");
                }
            }

            return new ListWrapper<ValidationError>();
        }

        public static ListWrapper<ValidationError> ValidateIsOnFloor(AppState appState, Entity entity)
        {
            foreach (CellCoordinates cellCoordinates in entity.cellCoordinatesList.bottomRow.items)
            {
                ListWrapper<Floor> floors = appState.Entities.Floors.queries.FindEntityTypesAtCell(cellCoordinates);

                if (floors.Count == 0)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must be placed on floor.");
                }
            }

            return new ListWrapper<ValidationError>();

        }

        public static ListWrapper<ValidationError> ValidateNotDirectlyNextToEntityOfSameType(AppState appState, Entity entity, CellCoordinates cellCoordinates)
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
            //     return new ListWrapper<ValidationError>($"{entity.definition.title} cannot be placed directly next to each other.");
            // }

            return new ListWrapper<ValidationError>();
        }

        public static ListWrapper<ValidationError> ValidateEntityIsNotOverlappingAnotherEntity(AppState appState, Entity entity)
        {
            IEntityStateSlice stateSlice = appState.Entities.GetStateSlice(entity);

            Entity overlappingEntity = stateSlice.entityList.Find((otherEntity) =>
                otherEntity != entity &&
                otherEntity.cellCoordinatesList.OverlapsWith(entity.cellCoordinatesList)
            );

            if (overlappingEntity != null)
            {
                return Validator.CreateSingleItemValidationErrorList($"You cannot build {entity.typeLabel}s on top of each other.");
            }

            return new ListWrapper<ValidationError>();
        }

        public static ListWrapper<ValidationError> ValidateEntityIsNotUnderground(AppState appState, Entity entity)
        {
            foreach (int floor in entity.cellCoordinatesList.floorValues)
            {
                if (floor < 0)
                {
                    return Validator.CreateSingleItemValidationErrorList($"You cannot build {entity.typeLabel} underground.");
                }
            }

            return new ListWrapper<ValidationError>();
        }

        /*
            Validation creators
        */
        public static EntityValidator.ValidationFunc CreateValidateEntityIsOnFloor(int floor) =>
            (AppState appState, Entity entity) =>
            {
                if (entity.cellCoordinatesList.lowestFloor != floor)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must be placed on floor {floor + 1}");
                }

                return new ListWrapper<ValidationError>();
            };

        public static EntityValidator.ValidationFunc CreateValidateEntityCellIsNotOnFloor(int floor) =>
            (AppState appState, Entity entity) =>
            {
                if (entity.cellCoordinatesList.lowestFloor == floor)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must not be placed on floor {floor + 1}");
                }

                return new ListWrapper<ValidationError>();
            };

        /* 
            Validations that return a bool not a list of ValidationErrors
        */
        // TODO - this still doesn't seem like the right place for this
        public static bool IsValidStandardLocation(AppState appState, CellCoordinates cellCoordinates) =>
            IsInsideRoom(appState, cellCoordinates)
                ? IsValidInsideEntityLocation(appState, cellCoordinates)
                : IsValidOutsideEntityLocation(appState, cellCoordinates);

        public static bool IsInsideRoom(AppState appState, CellCoordinates cellCoordinates) =>
            appState.Entities.Rooms.queries.FindEntityTypeAtCell(cellCoordinates) != null;

        // outside + on the ground
        public static bool IsValidOutsideEntityLocation(AppState appState, CellCoordinates cellCoordinates) =>
            cellCoordinates.floor == 0 &&
            HasEnoughVerticalSpace(appState, cellCoordinates);

        // inside + on a floor
        public static bool IsValidInsideEntityLocation(AppState appState, CellCoordinates cellCoordinates) =>
            appState.Entities.Rooms.queries.FindEntityTypeAtCell(cellCoordinates) != null &&
            appState.Entities.Floors.queries.FindEntityTypeAtCell(cellCoordinates) != null &&
            HasEnoughVerticalSpace(appState, cellCoordinates);

        public static bool HasEnoughVerticalSpace(AppState appState, CellCoordinates cellCoordinates) => true;
    }
}
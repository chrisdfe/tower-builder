using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Validators
{
    public delegate List<FurnitureValidationError> FurnitureValidationFunc(AppState appState, Furniture furniture);

    public static class GenericFurnitureValidations
    {
        public static List<FurnitureValidationError> ValidateWalletHasEnoughMoney(AppState appState, Furniture furniture)
        {
            return GenericValidations.ValidateWalletHasEnoughMoney<FurnitureValidationError>(appState, furniture.price);
        }

        public static List<FurnitureValidationError> ValidateIsInsideRoom(AppState appState, Furniture furniture)
        {
            Room furnitureRoom = appState.Rooms.queries.FindRoomAtCell(furniture.cellCoordinates);
            if (furnitureRoom == null)
            {
                return Helpers.CreateErrorList($"{furniture.title} must be placed inside.");
            }

            return Helpers.CreateEmptyErrorList();
        }

        public static FurnitureValidationFunc CreateValidateFurnitureIsOnFloor(int floor)
        {
            return (AppState appState, Furniture furniture) =>
            {
                if (furniture.cellCoordinates.floor != floor)
                {
                    return Helpers.CreateErrorList($"{furniture.title} must be placed on floor {floor + 1}");
                }

                return Helpers.CreateEmptyErrorList();
            };
        }

        public static FurnitureValidationFunc CreateValidateFurnitureIsNotOnFloor(int floor)
        {
            return (AppState appState, Furniture furniture) =>
            {
                if (furniture.cellCoordinates.floor == floor)
                {
                    return Helpers.CreateErrorList($"{furniture.title} must be not be placed on floor {floor + 1}");
                }

                return Helpers.CreateEmptyErrorList();
            };
        }

        static class Helpers
        {
            public static List<FurnitureValidationError> CreateEmptyErrorList()
            {
                return new List<FurnitureValidationError>();
            }

            public static List<FurnitureValidationError> CreateErrorList(string message)
            {
                return new List<FurnitureValidationError>() { new FurnitureValidationError(message) };
            }
        }
    }
}
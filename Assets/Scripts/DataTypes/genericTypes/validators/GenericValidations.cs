using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;

namespace TowerBuilder.DataTypes
{
    public static class GenericValidations
    {
        public static List<ValidationErrorType> ValidateWalletHasEnoughMoney<ValidationErrorType>(AppState appState, int price)
            where ValidationErrorType : ValidationErrorBase, new()
        {
            if (appState.Wallet.balance < price)
            {
                return Helpers.CreateErrorList<ValidationErrorType>("Insufficient funds.");
            }

            return Helpers.CreateEmptyErrorList<ValidationErrorType>();
        }

        static class Helpers
        {
            public static List<ValidationErrorType> CreateEmptyErrorList<ValidationErrorType>()
                where ValidationErrorType : ValidationErrorBase, new()
            {
                return new List<ValidationErrorType>();
            }

            public static List<ValidationErrorType> CreateErrorList<ValidationErrorType>(string message)
                where ValidationErrorType : ValidationErrorBase, new()
            {
                return new List<ValidationErrorType>() { new ValidationErrorType() { message = message } };
            }
        }
    }
}
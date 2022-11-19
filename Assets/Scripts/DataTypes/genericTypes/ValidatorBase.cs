using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;

namespace TowerBuilder.DataTypes
{
    public abstract class ValidationErrorBase
    {
        public string message;

        public ValidationErrorBase(string message)
        {
            this.message = message;
        }
    }

    public abstract class ValidatorBase<ValidationErrorType>
    {
        public List<ValidationErrorType> errors { get; protected set; } = new List<ValidationErrorType>();

        public bool isValid { get { return errors.Count == 0; } }

        public abstract void Validate(AppState appState);
    }
}
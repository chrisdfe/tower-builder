using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Validators
{
    public class ValidationError
    {
        public string message { get; } = "";

        public ValidationError(string message)
        {
            this.message = message;
        }
    }
}
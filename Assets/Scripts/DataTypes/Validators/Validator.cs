using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class Validator
    {
        /* 
            Static Interface
        */
        public static List<ValidationError> CreateSingleItemValidationErrorList(string message) =>
            new List<ValidationError>() { new ValidationError(message) };

        public static List<ValidationError> CreateEmptyValidationErrorList() =>
            new List<ValidationError>();
    }

    public class Validator<ItemType> : Validator
    {
        public delegate List<ValidationError> ValidationFunc(AppState appState, ItemType itemType);

        public ListWrapper<ValidationError> errors { get; protected set; } = new ListWrapper<ValidationError>();

        public bool isValid => errors.Count == 0;

        public virtual List<ValidationFunc> baseValidators =>
            new List<ValidationFunc>();

        protected virtual List<ValidationFunc> baseValidatorIgnoreList =>
            new List<ValidationFunc>();

        protected virtual List<ValidationFunc> customValidators => new List<ValidationFunc>();

        protected List<ValidationFunc> allValidators =>
            baseValidators
                .FindAll(validator => !baseValidatorIgnoreList.Contains(validator))
                .Concat(customValidators)
                .ToList();

        protected ItemType validationItem;

        public Validator(ItemType validationItem)
        {
            this.validationItem = validationItem;
        }

        public virtual void Validate(AppState appState)
        {
            errors = CreateValidationErrors(appState);
        }

        /* 
            Internals
        */
        ListWrapper<ValidationError> CreateValidationErrors(AppState appState) =>
            allValidators.Aggregate(new ListWrapper<ValidationError>(), (acc, validator) =>
                {
                    acc.Add(validator(appState, validationItem));
                    return acc;
                });
    }
}
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class Validator
    {
        /* 
            Static API
        */
        public static ListWrapper<ValidationError> CreateSingleItemValidationErrorList(string message) =>
            new ListWrapper<ValidationError>(new ValidationError(message));

        public static ListWrapper<ValidationError> CreateEmptyValidationErrorList() =>
            new ListWrapper<ValidationError>();
    }

    public class Validator<ItemType> : Validator
    {
        public delegate ListWrapper<ValidationError> ValidationFunc(AppState appState, ItemType itemType);

        public ListWrapper<ValidationError> errors { get; private set; } = new ListWrapper<ValidationError>();

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

        public void Validate(AppState appState)
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
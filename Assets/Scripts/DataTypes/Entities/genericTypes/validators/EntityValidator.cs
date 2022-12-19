using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public delegate EntityValidationErrorList EntityValidationFunc(AppState appState, Entity entity);

    public class EntityValidator
    {
        public EntityValidationErrorList errors { get; private set; } = new EntityValidationErrorList();

        public bool isValid => errors.Count == 0;

        public List<EntityValidationFunc> baseValidators =>
            new List<EntityValidationFunc>() {
                GenericEntityValidations.ValidateWalletHasEnoughMoney,
                GenericEntityValidations.ValidateEntityIsNotOverlappingAnotherEntity,
                GenericEntityValidations.ValidateEntityIsNotUnderground,
            };

        protected virtual List<EntityValidationFunc> baseValidatorIgnoreList =>
            new List<EntityValidationFunc>();

        protected virtual List<EntityValidationFunc> customValidators => new List<EntityValidationFunc>();

        protected List<EntityValidationFunc> allValidators =>
            baseValidators
                .FindAll(validator => !baseValidatorIgnoreList.Contains(validator))
                .Concat(customValidators)
                .ToList();

        Entity entity;

        public EntityValidator(Entity entity)
        {
            this.entity = entity;
        }

        public void Validate(AppState appState)
        {
            errors = allValidators.Aggregate(new EntityValidationErrorList(), (acc, validator) =>
            {
                acc.Add(validator(appState, entity));
                return acc;
            });
        }
    }
}
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroupBuildValidator : EntityGroupValidator
    {
        public EntityGroupBuildValidator(EntityGroup entityGroup) : base(entityGroup) { }

        protected override void ValidateDescendantEntities(AppState appState)
        {
            validationItem.GetDescendantEntities().ForEach((entity) =>
            {
                entity.buildValidator.Validate(appState);
            });
        }

        protected override void ValidateDescendantEntityGroups(AppState appState)
        {
            validationItem.GetDescendantEntityGroups().ForEach((entityGroup) =>
            {
                entityGroup.buildValidator.Validate(appState);
            });
        }

        protected override ListWrapper<ValidationError> GetDescendantEntityValidationErrors() =>
            validationItem
                .GetDescendantEntities().items
                .Aggregate(new ListWrapper<ValidationError>(), (acc, entity) =>
                {
                    acc.Add(entity.buildValidator.errors);
                    return acc;
                });

        protected override ListWrapper<ValidationError> GetDescendantEntityGroupValidationErrors() =>
            validationItem
                .GetDescendantEntityGroups().items
                .Aggregate(new ListWrapper<ValidationError>(), (acc, entity) =>
                {
                    acc.Add(entity.buildValidator.errors);
                    return acc;
                });
    }
}
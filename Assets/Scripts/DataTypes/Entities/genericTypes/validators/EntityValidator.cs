using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;

namespace TowerBuilder.DataTypes.Entities
{
    public delegate EntityValidationErrorList EntityValidationFunc(AppState appState, Entity entity);
    public delegate EntityValidationErrorList EntityCellValidationFunc(AppState appState, Entity entity, CellCoordinates cellCoordinates);

    public class EntityValidator
    {
        public EntityValidationErrorList errors { get; private set; } = new EntityValidationErrorList();

        public bool isValid => errors.Count == 0;

        // Validators that get run on every entity
        public static List<EntityValidationFunc> BaseValidators { get; } = new List<EntityValidationFunc>() {
          GenericEntityValidations.ValidateWalletHasEnoughMoney
        };

        public static List<EntityCellValidationFunc> BaseCellValidators { get; } = new List<EntityCellValidationFunc>() {
            GenericEntityCellValidations.ValidateEntityCellIsNotOverlappingAnotherEntity,
            GenericEntityCellValidations.ValidateEntityCellIsNotUnderground,
            // GenericEntityCellValidations.ValidateAcceptableOverhang,
        };

        protected virtual List<EntityValidationFunc> validators { get; } = new List<EntityValidationFunc>();
        protected virtual List<EntityCellValidationFunc> cellValidators { get; } = new List<EntityCellValidationFunc>();

        Entity entity;

        public EntityValidator(Entity entity)
        {
            this.entity = entity;
        }

        public void Validate(AppState appState)
        {
            errors = new EntityValidationErrorList();

            List<EntityValidationFunc> AllEntityValidators = BaseValidators.Concat(validators).ToList();
            List<EntityCellValidationFunc> AllEntityCellValidators = BaseCellValidators.Concat(cellValidators).ToList();

            foreach (EntityValidationFunc EntityValidationFunc in AllEntityValidators)
            {
                errors.Add(EntityValidationFunc(appState, entity));
            }

            entity.cellCoordinatesList.ForEach((cellCoordinates) =>
            {
                foreach (EntityCellValidationFunc EntityCellValidationFunc in AllEntityCellValidators)
                {
                    errors.Add(EntityCellValidationFunc(appState, entity, cellCoordinates));
                }
            });
        }
    }
}
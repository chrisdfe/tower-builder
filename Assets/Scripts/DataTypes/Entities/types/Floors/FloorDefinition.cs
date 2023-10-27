namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class FloorDefinition : EntityDefinition
    {
        public override ValidatorFactory buildValidatorFactory => (Entity entity) => new FloorValidator(entity as Floor);
    }
}
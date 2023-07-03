namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class ResidentDefinition : EntityDefinition
    {
        public override Resizability resizability => Resizability.Inflexible;
        public override ValidatorFactory buildValidatorFactory => (Entity entity) => new ResidentValidator(entity as Resident);
    }
}
namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class ResidentDefinition : EntityDefinition<Resident.Key>
    {
        public override Resizability resizability => Resizability.Inflexible;
        public override ValidatorFactory validatorFactory => (Entity entity) => new ResidentValidator(entity as Resident);
    }
}
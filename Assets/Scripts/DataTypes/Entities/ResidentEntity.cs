using TowerBuilder.DataTypes.Residents;

namespace TowerBuilder.DataTypes.Entities
{
    public class ResidentEntity : EntityBase
    {
        public Resident resident { get; private set; }

        public ResidentEntity(Resident resident)
        {
            this.resident = resident;
        }
    }
}
namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public enum ResidentBehaviorState
    {
        Idle,
        Walking
    }

    public class ResidentBehavior
    {
        Resident resident;

        public ResidentBehavior(Resident resident)
        {
            this.resident = resident;
        }
    }
}
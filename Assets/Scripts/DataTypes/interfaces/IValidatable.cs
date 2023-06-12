using TowerBuilder.ApplicationState;

namespace TowerBuilder.DataTypes
{
    public interface IValidatable
    {
        public bool isValid { get; }
        public ListWrapper<ValidationError> validationErrors { get; }
        public void Validate(AppState appState);
    }
}
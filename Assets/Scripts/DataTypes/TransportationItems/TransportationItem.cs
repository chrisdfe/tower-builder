namespace TowerBuilder.DataTypes.TransportationItems
{
    public class TransportationItem
    {
        public CellCoordinatesList cellCoordinatesList;

        public bool isInBlueprintMode = false;

        public TransportationItem(TransportationItemTemplate template)
        {

        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
        }
    }
}
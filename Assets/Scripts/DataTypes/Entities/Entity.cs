using System.Collections.Generic;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities
{
    public class Entity : ISetupable
    {
        public virtual string idKey { get => "entity"; }

        public int id { get; }

        public enum Key
        {
            Room,
            Resident,
            Furniture,
            TransportationItem
        }

        static EnumStringMap<Key> TypeLabels = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.Room,               "Room" },
                { Key.Resident,           "Resident" },
                { Key.Furniture,          "Furniture" },
                { Key.TransportationItem, "Transportation Item" }
            }
        );

        public string key { get; protected set; } = "None";
        public string title { get; protected set; } = "None";
        public string category { get; protected set; } = "None";

        public virtual int pricePerCell { get => 0; }

        public bool isInBlueprintMode { get; set; } = false;

        public CellCoordinatesList cellCoordinatesList { get; set; } = new CellCoordinatesList();

        public Entity()
        {
            this.id = UIDGenerator.Generate(idKey);
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public void OnDestroy() { }
        public virtual void Setup() { }
        public virtual void Teardown() { }
    }
}
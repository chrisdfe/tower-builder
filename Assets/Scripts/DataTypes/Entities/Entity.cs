using System.Collections.Generic;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities
{
    public class Entity : ISetupable
    {
        public virtual string idKey { get => "entity"; }

        public int id { get; }

        public enum Type
        {
            Room,
            Resident,
            Furniture,
            TransportationItem
        }

        static EnumStringMap<Type> TypeLabels = new EnumStringMap<Type>(
            new Dictionary<Type, string>() {
                { Type.Room,               "Room" },
                { Type.Resident,           "Resident" },
                { Type.Furniture,          "Furniture" },
                { Type.TransportationItem, "Transportation Item" }
            }
        );

        public string title { get; } = "None";
        public string category { get; } = "None";

        public virtual int pricePerCell => 0;

        public int price => pricePerCell * cellCoordinatesList.Count;

        public bool isInBlueprintMode { get; set; } = false;

        // TODO - remove the set; accessor too
        public CellCoordinatesList cellCoordinatesList { get; set; } = new CellCoordinatesList();

        public EntityTemplate template { get; }

        public Entity(EntityTemplate template)
        {
            this.template = template;

            this.id = UIDGenerator.Generate(idKey);
            this.title = template.title;
            this.category = template.category;

            this.cellCoordinatesList = template.cellCoordinatesList.Clone();
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public void OnDestroy() { }
        public virtual void Setup() { }
        public virtual void Teardown() { }
    }

    public class Entity<KeyType> : Entity
        where KeyType : struct
    {
        public KeyType key { get; protected set; }

        public Entity(EntityTemplate<KeyType> template) : base(template as EntityTemplate)
        {
            this.key = template.key;
        }
    }
}
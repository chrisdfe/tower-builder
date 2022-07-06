using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furniture;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.Templates
{
    public class FurnitureTemplates
    {
        public List<FurnitureTemplate> furnitureTemplates { get; private set; } = new List<FurnitureTemplate>();

        public FurnitureTemplates()
        {
            // RegisterTemplates(DefaultFurnitureTemplates.FurnitureTemplates);
        }

        // Registration
        public void RegisterTemplate(FurnitureTemplate furnitureTemplate)
        {
            this.furnitureTemplates.Add(furnitureTemplate);
        }

        public void RegisterTemplates(List<FurnitureTemplate> furnitureTemplates)
        {
            this.furnitureTemplates = this.furnitureTemplates.Concat(furnitureTemplates).ToList();
        }
    }
}
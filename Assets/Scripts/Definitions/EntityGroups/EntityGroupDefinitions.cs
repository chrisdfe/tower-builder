using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Freights;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.InteriorLights;
using TowerBuilder.DataTypes.Entities.InteriorWalls;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Entities.Windows;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.EntityGroups.Vehicles;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class EntityGroupDefinitions
    {
        public static Dictionary<System.Type, System.Type> EntityDefinitionEntityTypeMap = new Dictionary<System.Type, System.Type>()
        {
            // { typeof(InteriorWallDefinition), typeof(InteriorWall) },
        };

        public DefinitionQueries Queries { get; }

        // public RoomDefinitionsList Rooms = new RoomDefinitionsList();

        public Dictionary<Type, IEntityDefinitionsList> entityDefinitionsMap { get; }

        public EntityGroupDefinitions()
        {
            // Queries = new DefinitionQueries(this);

            // entityDefinitionsMap = new Dictionary<Type, IEntityDefinitionsList>() {
            //     { typeof(Floor), Floors },
            // };
        }

        public class DefinitionQueries
        {

        }
    }
}
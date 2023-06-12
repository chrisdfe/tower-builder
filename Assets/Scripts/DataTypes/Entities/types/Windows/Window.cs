using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Windows
{
    public class Window : Entity
    {
        public override string idKey => "windows";

        public override string typeLabel => "Window";

        public Window(WindowDefinition windowDefinition) : base(windowDefinition) { }
    }
}
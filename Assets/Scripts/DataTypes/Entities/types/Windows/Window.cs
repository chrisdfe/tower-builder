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

        public Window() : base() { }
        public Window(Input input) : base(input) { }
        public Window(WindowDefinition windowDefinition) : base(windowDefinition) { }
    }
}
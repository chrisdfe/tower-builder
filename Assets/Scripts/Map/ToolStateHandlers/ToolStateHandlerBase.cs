using System;
using TowerBuilder;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class ToolStateHandlerBase
    {
        void Update() { }

        void OnMouseDown() { }

        void OnMouseUp() { }

        void OnTransitionFrom(ToolState previousToolState) { }

        void OnTransitionTo() { }
    }
}
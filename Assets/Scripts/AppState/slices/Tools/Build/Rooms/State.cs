using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools.Build.Rooms
{
    public class State : BuildModeStateBase
    {
        public struct Input { }

        public class Events
        {

        }

        public Events events { get; private set; }

        public string selectedRoomKey { get; private set; } = null;

        public State(AppState appState, Tools.State state, Build.State buildState, Input input) : base(appState, state, buildState)
        {
            events = new Events();
        }

        public void SetSelectedRoomKey(string value)
        {
            Debug.Log("here I am");
            Debug.Log("in state");
            Debug.Log(value);
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {

        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {

        }

        public override void OnSelectionEnd(SelectionBox selectionBox)
        {

        }
    }
}
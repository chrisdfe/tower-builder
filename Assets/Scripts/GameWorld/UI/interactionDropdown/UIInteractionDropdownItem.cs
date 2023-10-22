using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class UIInteractionDropdownItem : MonoBehaviour
    {
        public delegate void OnClick();

        public class Input
        {
            public string label;
            public OnClick onClick;
        }

        string label;
        OnClick onClick;

        public UIInteractionDropdownItem(Input input)
        {
            onClick = input.onClick;
            label = input.label;
        }

        public UIInteractionDropdownItem Create(Input input)
        {
            return null;
        }
    }
}
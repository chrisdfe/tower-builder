using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class UIInteractionDropdownItem : MonoBehaviour
    {
        public delegate void OnClick();

        string label;
        OnClick onClick;

        public UIInteractionDropdownItem(string label, OnClick onClick)
        {
            this.onClick = onClick;
            this.label = label;
        }
    }
}
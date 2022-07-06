using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using TowerBuilder.Templates;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class BuildStateButtonsManager : MonoBehaviour
    {
        Color originalColor;
        Button currentButton;

        Transform entityGroupButtonsWrapper;

        RoomEntityGroupButtons roomEntityGroupButtons;

        void Awake()
        {
            entityGroupButtonsWrapper = transform.Find("EntityGroupButtons");

            Transform roomEntityGroupButtonsWrapper = transform.Find("RoomEntityGroupButtons");
            Transform furnitureEntityGroupButtonsWrapper = transform.Find("FurnitureEntityGroupButtons");
            Transform residentEntityGroupButtonsWrapper = transform.Find("ResidentEntityGroupButtons");

            // entityGroupPanels = new Dictionary<EntityType, EntityGroupPanelWrapper>() {
            //     { EntityType.Room, new RoomEntityGroupPanel(entityGroupButtonsWrapper, roomEntityGroupButtons) },
            //     { EntityType.Furniture, new FurnitureEntityGroupPanel(entityGroupButtonsWrapper, furnitureEntityGroupButtons) },
            //     { EntityType.Resident, new ResidentEntityGroupPanel(entityGroupButtonsWrapper, residentEntityGroupButtons) },
            // };

            roomEntityGroupButtons = new RoomEntityGroupButtons(roomEntityGroupButtonsWrapper);

            TransformUtils.DestroyChildren(entityGroupButtonsWrapper);
            UISelectButton roomEntityGroupButton = UISelectButton.Create(new UISelectButton.Input() { label = "rooms", value = "rooms" });
            roomEntityGroupButton.transform.SetParent(entityGroupButtonsWrapper, false);
            roomEntityGroupButton.onClick += OnEntityGroupButtonClick;
        }

        void OnEntityGroupButtonClick(string value)
        {
            // if value is "rooms":
            roomEntityGroupButtons.Setup();
        }
    }
}

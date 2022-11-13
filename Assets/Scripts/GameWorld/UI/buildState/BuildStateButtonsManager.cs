using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.Definitions.Templates;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
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
        FurnitureEntityGroupButtons furnitureEntityGroupButtons;
        ResidentEntityGroupButtons residentEntityGroupButtons;

        UISelectButton roomEntityGroupButton;
        UISelectButton furnitureEntityGroupButton;
        UISelectButton residentEntityGroupButton;

        string currentCategory = "";

        void Awake()
        {
            entityGroupButtonsWrapper = transform.Find("EntityGroupButtons");

            Transform roomEntityGroupButtonsWrapper = transform.Find("RoomEntityGroupButtons");
            Transform furnitureEntityGroupButtonsWrapper = transform.Find("FurnitureEntityGroupButtons");
            Transform residentEntityGroupButtonsWrapper = transform.Find("ResidentEntityGroupButtons");

            roomEntityGroupButtons = new RoomEntityGroupButtons(roomEntityGroupButtonsWrapper);
            furnitureEntityGroupButtons = new FurnitureEntityGroupButtons(furnitureEntityGroupButtonsWrapper);
            residentEntityGroupButtons = new ResidentEntityGroupButtons(residentEntityGroupButtonsWrapper);

            CreateEntityGroupButtons();
        }

        void CreateEntityGroupButtons()
        {
            TransformUtils.DestroyChildren(entityGroupButtonsWrapper);

            roomEntityGroupButton = CreateEntityGroupButton("rooms");
            furnitureEntityGroupButton = CreateEntityGroupButton("furniture");
            residentEntityGroupButton = CreateEntityGroupButton("residents");

            UISelectButton CreateEntityGroupButton(string value)
            {
                UISelectButton entityButton = UISelectButton.Create(new UISelectButton.Input() { label = value, value = value });
                entityButton.transform.SetParent(entityGroupButtonsWrapper, false);
                entityButton.onClick += OnEntityGroupButtonClick;
                return entityButton;
            }
        }

        void OnEntityGroupButtonClick(string newCategory)
        {
            TeardownCurrentCategory();
            currentCategory = newCategory;

            switch (newCategory)
            {
                case "rooms":
                    roomEntityGroupButton.SetSelected(true);
                    roomEntityGroupButtons.Setup();
                    Registry.appState.Tools.buildToolState.SetSelectedEntityType(EntityType.Room);
                    break;
                case "furniture":
                    Registry.appState.Tools.buildToolState.SetSelectedEntityType(EntityType.Furniture);
                    break;
                case "residents":
                    Registry.appState.Tools.buildToolState.SetSelectedEntityType(EntityType.Resident);
                    break;
            }

        }

        void TeardownCurrentCategory()
        {
            switch (currentCategory)
            {
                case "rooms":
                    roomEntityGroupButtons.Teardown();
                    roomEntityGroupButton.SetSelected(false);
                    break;
                case "furniture":
                    furnitureEntityGroupButtons.Teardown();
                    furnitureEntityGroupButton.SetSelected(false);
                    break;
                case "residents":
                    residentEntityGroupButtons.Teardown();
                    residentEntityGroupButton.SetSelected(false);
                    break;
            }

            currentCategory = "";
        }
    }
}

using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI.Components;
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
        // FurnitureEntityGroupButtons furnitureEntityGroupButtons;
        // ResidentEntityGroupButtons residentEntityGroupButtons;
        // TransportationItemEntityGroupButtons transportationItemEntityGroupButtons;

        UISelectButton roomEntityGroupButton;
        UISelectButton furnitureEntityGroupButton;
        UISelectButton residentEntityGroupButton;
        UISelectButton transportationItemEntityGroupButton;

        string currentCategory = "";

        void Awake()
        {
            entityGroupButtonsWrapper = transform.Find("EntityGroupButtons");

            Transform roomEntityGroupButtonsWrapper = transform.Find("RoomEntityGroupButtons");
            Transform furnitureEntityGroupButtonsWrapper = transform.Find("FurnitureEntityGroupButtons");
            Transform residentEntityGroupButtonsWrapper = transform.Find("ResidentEntityGroupButtons");
            Transform transportationitemEntityGroupButtonsWrapper = transform.Find("TransportationItemEntityGroupButtons");

            roomEntityGroupButtons = new RoomEntityGroupButtons(roomEntityGroupButtonsWrapper);
            // furnitureEntityGroupButtons = new FurnitureEntityGroupButtons(furnitureEntityGroupButtonsWrapper);
            // residentEntityGroupButtons = new ResidentEntityGroupButtons(residentEntityGroupButtonsWrapper);
            // transportationItemEntityGroupButtons = new TransportationItemEntityGroupButtons(transportationitemEntityGroupButtonsWrapper);

            CreateEntityGroupButtons();
        }

        void CreateEntityGroupButtons()
        {
            TransformUtils.DestroyChildren(entityGroupButtonsWrapper);

            roomEntityGroupButton = CreateEntityGroupButton("rooms");
            furnitureEntityGroupButton = CreateEntityGroupButton("furniture");
            residentEntityGroupButton = CreateEntityGroupButton("residents");
            transportationItemEntityGroupButton = CreateEntityGroupButton("transportation");

            UISelectButton CreateEntityGroupButton(string value)
            {
                UISelectButton entityButton = UISelectButton.Create(entityGroupButtonsWrapper, new UISelectButton.Input() { label = value, value = value });
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
                    Registry.appState.Tools.buildToolState.SetSelectedEntityKey(Entity.Type.Room);
                    break;
                case "furniture":
                    furnitureEntityGroupButton.SetSelected(true);
                    // furnitureEntityGroupButtons.Setup();
                    Registry.appState.Tools.buildToolState.SetSelectedEntityKey(Entity.Type.Furniture);
                    break;
                case "residents":
                    Registry.appState.Tools.buildToolState.SetSelectedEntityKey(Entity.Type.Resident);
                    break;
                case "transportation":
                    Registry.appState.Tools.buildToolState.SetSelectedEntityKey(Entity.Type.TransportationItem);
                    transportationItemEntityGroupButton.SetSelected(true);
                    // transportationItemEntityGroupButtons.Setup();
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
                    // furnitureEntityGroupButtons.Teardown();
                    furnitureEntityGroupButton.SetSelected(false);
                    break;
                case "residents":
                    // residentEntityGroupButtons.Teardown();
                    residentEntityGroupButton.SetSelected(false);
                    break;
                case "transportation":
                    // transportationItemEntityGroupButtons.Teardown();
                    transportationItemEntityGroupButton.SetSelected(false);
                    break;
            }

            currentCategory = "";
        }
    }
}

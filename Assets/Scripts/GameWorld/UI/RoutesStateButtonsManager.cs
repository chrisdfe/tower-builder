using TowerBuilder.Stores;

using TowerBuilder.Stores.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class RoutesStateButtonsManager : MonoBehaviour
    {
        static Color PRESSED_COLOR = Color.red;


        Button routeStartButton;
        Button routeEndButton;
        Button routeCalculateButton;
        Button routeClearButton;

        Color originalColor;
        Button currentButton;

        void Awake()
        {
            routeStartButton = transform.Find("RouteStartButton").GetComponent<Button>();
            routeEndButton = transform.Find("RouteEndButton").GetComponent<Button>();
            routeCalculateButton = transform.Find("RouteCalculateButton").GetComponent<Button>();
            routeClearButton = transform.Find("RouteClearButton").GetComponent<Button>();

            routeStartButton.onClick.AddListener(OnRouteStartButtonClick);
            routeEndButton.onClick.AddListener(OnRouteEndButtonClick);
            routeCalculateButton.onClick.AddListener(OnRouteCalculateButtonClick);
            routeClearButton.onClick.AddListener(OnRouteClearButtonClick);

            originalColor = routeStartButton.colors.normalColor;

            // SelectButton(Registry.Stores.UI.buildToolSubState.selectedRoomKey);
            // Registry.Stores.UI.buildToolSubState.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void OnRouteStartButtonClick()
        {
            Registry.Stores.UI.routesToolSubState.SetClickState(RoutesToolState.ClickState.RouteStart);
        }

        void OnRouteEndButtonClick()
        {
            Registry.Stores.UI.routesToolSubState.SetClickState(RoutesToolState.ClickState.RouteEnd);
        }

        void OnRouteCalculateButtonClick()
        {
            Registry.Stores.Routes.CalculateDebugRoute();
        }

        void OnRouteClearButtonClick()
        {
            Registry.Stores.Routes.ClearDebugRoute();
            Registry.Stores.UI.routesToolSubState.SetClickState(RoutesToolState.ClickState.None);
        }
    }
}

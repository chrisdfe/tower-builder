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
        Button addDebugResidentButton;
        Button nextPointOnRouteButton;
        Button resetRouteProgressButton;

        Color originalColor;
        Button currentButton;

        void Awake()
        {
            routeStartButton = transform.Find("RouteStartButton").GetComponent<Button>();
            routeEndButton = transform.Find("RouteEndButton").GetComponent<Button>();
            routeCalculateButton = transform.Find("RouteCalculateButton").GetComponent<Button>();
            routeClearButton = transform.Find("RouteClearButton").GetComponent<Button>();
            addDebugResidentButton = transform.Find("AddDebugResidentButton").GetComponent<Button>();
            nextPointOnRouteButton = transform.Find("NextPointOnRouteButton").GetComponent<Button>();
            resetRouteProgressButton = transform.Find("ResetRouteProgressButton").GetComponent<Button>();

            routeStartButton.onClick.AddListener(OnRouteStartButtonClick);
            routeEndButton.onClick.AddListener(OnRouteEndButtonClick);
            routeCalculateButton.onClick.AddListener(OnRouteCalculateButtonClick);
            routeClearButton.onClick.AddListener(OnRouteClearButtonClick);
            addDebugResidentButton.onClick.AddListener(OnAddDebugResidentButtonClick);
            nextPointOnRouteButton.onClick.AddListener(OnNextPointOnRouteButtonClick);
            resetRouteProgressButton.onClick.AddListener(OnResetRouteProgressButtonClick);

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

        void OnAddDebugResidentButtonClick()
        {
            CellCoordinates routeStartCoordinates = Registry.Stores.Routes.debugRouteStartCoordinates;

            if (routeStartCoordinates == null)
            {
                Debug.Log("route start coordinates need to be set first");
                return;
            }

            if (Registry.Stores.Routes.debugRouteEndCoordinates == null)
            {
                Debug.Log("route end coordinates need to be set first");
                return;
            }

            if (Registry.Stores.Routes.debugRouteAttempts == null)
            {
                Debug.Log("no route attempts.");
                return;
            }

            Registry.Stores.Residents.CreateDebugResidentAtCoordinates(routeStartCoordinates);
        }

        void OnNextPointOnRouteButtonClick()
        {
            Registry.Stores.Residents.AdvanceDebugResidentAlongRoute();
        }

        void OnResetRouteProgressButtonClick()
        {
            Debug.Log("reset route progress");
        }
    }
}

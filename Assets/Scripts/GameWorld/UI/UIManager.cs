using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class UIManager : MonoBehaviour, IFindable
    {
        public AssetList assetList = new AssetList();

        public bool mouseIsOverUI { get; private set; }

        public ModalsManager modalsManager { get; private set; }
        public DropdownsManager dropdownsManager { get; private set; }

        public CanvasScaler canvasScaler { get; private set; }

        Canvas canvas;
        GraphicRaycaster graphicRaycaster;
        PointerEventData pointerEventData;
        EventSystem eventSystem;


        /*
            Public Interface
        */
        public void Start()
        {
            canvas = transform.Find("Canvas").GetComponent<Canvas>();
            canvasScaler = canvas.GetComponent<CanvasScaler>();
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            eventSystem = canvas.GetComponent<EventSystem>();

            modalsManager = canvas.transform.Find("ModalsManager").GetComponent<ModalsManager>();
            dropdownsManager = canvas.transform.Find("DropdownsManager").GetComponent<DropdownsManager>();

            // Calculate layermask to Raycast to. (Raycast to "cube" && "sphere" layers only)
            int uiLayerIndex = LayerMask.NameToLayer("UI");
            // int layerMask = (1 << uiLayerIndex);
            graphicRaycaster.blockingMask = uiLayerIndex;
        }

        public void Update()
        {
            SetMouseIsOverUI();

            if (Input.GetMouseButtonDown(0))
            {
                OnLeftMouseButtonDown();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnLeftMouseButtonUp();
            }

            if (Input.GetMouseButtonDown(1))
            {
            }

            if (Input.GetMouseButtonUp(1))
            {
                OnRightMouseButtonUp();
            }

            if (Input.GetButtonDown("Escape"))
            {
                modalsManager.ToggleDebugModal();
            }
        }

        public void OnLeftMouseButtonDown()
        {
            if (mouseIsOverUI) return;

            Registry.appState.UI.SelectStart();
        }

        public void OnLeftMouseButtonUp()
        {
            Registry.appState.UI.SelectEnd();
        }

        public void OnRightMouseButtonDown() { }

        public void OnRightMouseButtonUp()
        {
            if (Registry.appState.Tools.currentKey == State.Key.Inspect)
            {
                if (Registry.appState.Tools.Inspect.inspectedEntity != null && Registry.appState.Tools.Inspect.inspectedEntity.GetType() == typeof(Resident))
                {
                    Resident inspectedResident = Registry.appState.Tools.Inspect.inspectedEntity as Resident;
                    CellCoordinates selectedCell = Registry.appState.UI.selectedCell;

                    // TODO - 'build interaction options' function
                    dropdownsManager.interactionDropdown.SetItemsAndOpen(new List<UIInteractionDropdownItem.Input>() {
                        new UIInteractionDropdownItem.Input() {
                            label = "Go here",
                            onClick = () => {
                                Debug.Log("sending " + inspectedResident + " to " + selectedCell);
                                Registry.appState.Entities.Residents.SendResidentTo(inspectedResident, selectedCell);
                            }
                        }
                    });
                }
            }

            return;
        }

        /*
            Internals
        */
        void SetMouseIsOverUI()
        {
            //Set up the new Pointer Event
            pointerEventData = new PointerEventData(eventSystem);
            //Set the Pointer Event Position to that of the mouse position
            pointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            graphicRaycaster.Raycast(pointerEventData, results);

            mouseIsOverUI = results.Count > 0;
        }

        public static UIManager Find() =>
            GameObject.Find("UIManager").GetComponent<UIManager>();
    }
}
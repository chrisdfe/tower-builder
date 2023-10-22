using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class UIManager : MonoBehaviour, IFindable
    {
        public AssetList assetList = new AssetList();

        public bool mouseIsOverUI { get; private set; }

        Canvas canvas;
        public CanvasScaler canvasScaler { get; private set; }

        GraphicRaycaster graphicRaycaster;
        PointerEventData pointerEventData;
        EventSystem eventSystem;

        Transform modalsWrapper;
        DebugModalManager debugModalManager;

        /*
            Public Interface
        */
        public void Start()
        {
            canvas = transform.Find("Canvas").GetComponent<Canvas>();
            canvasScaler = canvas.GetComponent<CanvasScaler>();
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            eventSystem = canvas.GetComponent<EventSystem>();

            modalsWrapper = canvas.transform.Find("ModalsWrapper");
            debugModalManager = modalsWrapper.Find("DebugModal").GetComponent<DebugModalManager>();

            // Calculate layermask to Raycast to. (Raycast to "cube" && "sphere" layers only)
            int uiLayerIndex = LayerMask.NameToLayer("UI");
            int layerMask = (1 << uiLayerIndex);
            graphicRaycaster.blockingMask = uiLayerIndex;
        }

        public void Update()
        {
            SetMouseIsOverUI();
        }

        /*
            Public Interface
        */
        public void ToggleDebugModal()
        {
            debugModalManager.Toggle();
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
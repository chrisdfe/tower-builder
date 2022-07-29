using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Blueprints;
using TowerBuilder.State.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class UIManager : MonoBehaviour
    {
        public bool mouseIsOverUI { get; private set; }

        Canvas canvas;
        GraphicRaycaster graphicRaycaster;
        PointerEventData pointerEventData;
        EventSystem eventSystem;

        void Start()
        {
            canvas = transform.Find("Canvas").GetComponent<Canvas>();
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            eventSystem = canvas.GetComponent<EventSystem>();
        }

        void Update()
        {
            //Check if the left Mouse button is clicked
            if (Input.GetKey(KeyCode.Mouse0))
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
        }

        public static UIManager Find()
        {
            return GameObject.Find("UIManager").GetComponent<UIManager>();
        }
    }
}
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldTransportationItemList : MonoBehaviour
    {
        List<GameWorldTransportationItem> gameWorldTransportationItemList = new List<GameWorldTransportationItem>();

        /*
            Lifecycle Methods
        */
        void Awake()
        {
            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        void Setup()
        {
            Registry.appState.Entities.TransportationItems.events.onItemsAdded += OnTransportationItemsAdded;
            Registry.appState.Entities.TransportationItems.events.onItemsRemoved += OnTransportationItemsRemoved;
            Registry.appState.Entities.TransportationItems.events.onItemsBuilt += OnTransportationItemsBuilt;
        }

        void Teardown()
        {
            Registry.appState.Entities.TransportationItems.events.onItemsAdded -= OnTransportationItemsAdded;
            Registry.appState.Entities.TransportationItems.events.onItemsRemoved -= OnTransportationItemsRemoved;
            Registry.appState.Entities.TransportationItems.events.onItemsBuilt -= OnTransportationItemsBuilt;
        }

        /* 
            Internals
        */
        void CreateTransportationItem(TransportationItem transportationItem)
        {
            GameWorldTransportationItem gameWorldTransportationItem = GameWorldTransportationItem.Create(transform);
            gameWorldTransportationItem.transportationItem = transportationItem;
            gameWorldTransportationItem.transportationItem = transportationItem;
            gameWorldTransportationItem.Setup();
            gameWorldTransportationItemList.Add(gameWorldTransportationItem);
        }

        void RemoveTransportationItem(TransportationItem transportationItem)
        {
            GameWorldTransportationItem gameWorldTransportationItem =
                gameWorldTransportationItemList.Find(gameWorldTransportationItem =>
                    gameWorldTransportationItem.transportationItem == transportationItem
                );
            gameWorldTransportationItem.Teardown();
            GameObject.Destroy(gameWorldTransportationItem.gameObject);
            gameWorldTransportationItemList.Remove(gameWorldTransportationItem);
        }

        /* 
            Event Handlers
        */
        void OnTransportationItemsAdded(TransportationItemsList transportationItemList)
        {
            transportationItemList.ForEach(transportationItem =>
            {
                CreateTransportationItem(transportationItem);
            });
        }

        void OnTransportationItemsRemoved(TransportationItemsList transportationItemList)
        {
            transportationItemList.ForEach(transportationItem =>
            {
                RemoveTransportationItem(transportationItem);
            });
        }

        void OnTransportationItemsBuilt(TransportationItemsList transportationItemList)
        {
        }
    }
}
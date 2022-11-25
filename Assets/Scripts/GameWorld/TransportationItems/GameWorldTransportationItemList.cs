using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.TransportationItems;
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
            Registry.appState.TransportationItems.events.onTransportationItemAdded += OnTransportationItemAdded;
            Registry.appState.TransportationItems.events.onTransportationItemRemoved += OnTransportationItemRemoved;
            Registry.appState.TransportationItems.events.onTransportationItemBuilt += OnTransportationItemBuilt;
        }

        void Teardown()
        {
            Registry.appState.TransportationItems.events.onTransportationItemAdded -= OnTransportationItemAdded;
            Registry.appState.TransportationItems.events.onTransportationItemRemoved -= OnTransportationItemRemoved;
            Registry.appState.TransportationItems.events.onTransportationItemBuilt -= OnTransportationItemBuilt;
        }

        /* 
            Internals
        */
        void CreateTransportationItem(TransportationItem transportationItem)
        {
            GameWorldTransportationItem gameWorldTransportationItem = GameWorldTransportationItem.Create(transform);
            gameWorldTransportationItem.transportationItem = transportationItem;
            gameWorldTransportationItem.Setup();
            gameWorldTransportationItemList.Add(gameWorldTransportationItem);
        }

        void RemoveTransportationtItem(TransportationItem transportationItem)
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
        void OnTransportationItemAdded(TransportationItem transportationItem)
        {
            Debug.Log("Added");
            CreateTransportationItem(transportationItem);
        }

        void OnTransportationItemRemoved(TransportationItem transportationItem)
        {
            Debug.Log("Removed");
            RemoveTransportationtItem(transportationItem);
        }

        void OnTransportationItemBuilt(TransportationItem transportationItem)
        {
            Debug.Log("Built");
        }

    }
}
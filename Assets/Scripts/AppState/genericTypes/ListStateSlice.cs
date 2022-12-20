using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState
{
    public delegate void ListEvent<ItemType>(ListWrapper<ItemType> list);
    public delegate void ItemEvent<ItemType>(ItemType item);

    public interface IListStateSlice<ItemType>
    {
        // public interface IEvents
        // {
        //     public ListEvent<ItemType> onItemsAdded { get; set; }
        //     public ListEvent<ItemType> onItemsRemoved { get; set; }
        //     public ListEvent<ItemType> onListUpdated { get; set; }
        // }

        // TODO - fix this covariance issue (ListWrapper<ItemType> != ListWrapperType)
        public ListWrapper<ItemType> list { get; }

        // public IEvents events { get; }

        public void Add(ListWrapper<ItemType> newItemsList);

        public void Add(ItemType item);

        public void Remove(ListWrapper<ItemType> removedItemsList);

        public void Remove(ItemType item);
    }

    [Serializable]
    public class ListStateSlice<ListWrapperType, ItemType, EventsType> : StateSlice
        where ListWrapperType : ListWrapper<ItemType>, new()
        where EventsType : ListStateSlice<ListWrapperType, ItemType, EventsType>.Events, new()
    {
        public ListWrapperType list { get; protected set; } = new ListWrapperType();

        public class Events
        {
            public delegate void ListEvent(ListWrapperType list);
            public ListEvent onItemsAdded;
            public ListEvent onItemsRemoved;
            public ListEvent onItemsUpdated;

            public ListEvent onListUpdated;

            public delegate void ItemEvent(ItemType item);
        }

        public virtual EventsType events { get; } = new EventsType();

        public ListStateSlice(AppState appState) : base(appState) { }

        public virtual void Add(ListWrapperType newItemsList)
        {
            list.Add(newItemsList);

            events.onItemsAdded?.Invoke(newItemsList);
            events.onListUpdated?.Invoke(list);
        }

        public virtual void Add(ItemType item)
        {
            ListWrapperType newItemsList = new ListWrapperType();
            newItemsList.Add(item);
            Add(newItemsList);
        }

        public virtual void Remove(ListWrapperType removedItemsList)
        {
            list.Remove(removedItemsList);
            events.onItemsRemoved?.Invoke(removedItemsList);
            events.onListUpdated?.Invoke(list);
        }

        public virtual void Remove(ItemType item)
        {
            ListWrapperType removedItemsList = new ListWrapperType();
            removedItemsList.Add(item);
            Remove(removedItemsList);
        }
    }
}

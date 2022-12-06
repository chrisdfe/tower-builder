using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState
{
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
            public ListEvent onItemsBuilt;
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

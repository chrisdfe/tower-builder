using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TowerBuilder.Systems;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class SaveableListWrapper<ItemType, ItemInputType> : ListWrapper<ItemType>, ISaveable
        where ItemType : class, ISaveable, new()
        where ItemInputType : SaveableInputBase
    {
        public class Input : SaveableInputBase
        {
            public List<ItemInputType> items;
        }

        public virtual Type GetInputType() => typeof(Input);

        public SaveableListWrapper() { }

        public SaveableListWrapper(ItemType item)
        {
            items.Add(item);
        }

        public SaveableListWrapper(List<ItemType> items)
        {
            this.items = items;
        }

        public SaveableListWrapper(SaveableListWrapper<ItemType, ItemInputType> itemList)
        {
            items = items.Concat(itemList.items).ToList();
        }

        public SaveableListWrapper(SaveableInputBase input)
        {
            ConsumeInput(input);
        }

        public virtual SaveableInputBase ToInput()
        {
            var inputItems =
                items.Select(item => item.ToInput() as ItemInputType).ToList();

            return new Input()
            {
                items = inputItems
            };
        }

        public virtual void ConsumeInput(SaveableInputBase rawInput)
        {
            Debug.Log("rawInput is Input");
            Debug.Log(rawInput is Input);

            var input = (Input)rawInput;

            Debug.Log("rawInput");
            Debug.Log(rawInput);
            Debug.Log("input");
            Debug.Log(input);

            items =
                input.items.ToList()
                .Select((itemInput) =>
                {
                    var item = Activator.CreateInstance<ItemType>();
                    item.ConsumeInput(itemInput);
                    return item;
                }).ToList();
        }
    }
}
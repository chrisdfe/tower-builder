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
    public class SaveableListWrapper<ItemType> : ListWrapper<ItemType>, ISaveable
        where ItemType : class, ISaveable
    {
        public class Input : SaveableInputBase
        {
            public List<ItemType> items;

            public Input() : base() { }

            public Input(object rawInput) : base(rawInput)
            {
                List<object> castRawInput = (List<object>)rawInput;
                items = castRawInput.ConvertAll<ItemType>(item => item as ItemType);
            }

            public override object ToRawInput() => items;
        }

        public SaveableListWrapper() { }

        public SaveableListWrapper(ItemType item)
        {
            this.items.Add(item);
        }

        public SaveableListWrapper(List<ItemType> items)
        {
            this.items = items;
        }

        public SaveableListWrapper(SaveableListWrapper<ItemType> itemList)
        {
            this.items = this.items.Concat(itemList.items).ToList();
        }

        public SaveableListWrapper(Input input)
        {
            ConsumeInput(input);
        }

        public SaveableInputBase ToInput()
        {
            Debug.Log("list wrapper ToInput");

            var inputItems =
                items.Select(item => item.ToInput()).ToList();

            return new Input()
            {
                items = items
            };
        }

        public void ConsumeInput(SaveableInputBase baseInput)
        {
            Input input = (Input)baseInput;

            // this.items =
            //     input.items.ToList()
            //     .Select((item) =>
            //     {
            //         Debug.Log("item");
            //         Debug.Log(item);

            //         var FromInputMethod = typeof(ItemType).GetMethod("FromInput");
            //         if (FromInputMethod != null)
            //         {
            //             // Debug.Log(item.GetType());
            //             // JObject itemAsJObject = (JObject)item;
            //             // object itemAsObject = itemAsJObject.Cast<object>();

            //             // Debug.Log(typeof(item as object));
            //             return FromInputMethod.Invoke(null, new object[] { item }) as ItemType;
            //         }

            //         ItemType itemType = null;
            //         // TODO - check if ItemType is constructable with 0 arguments
            //         ConstructorInfo[] ctors = typeof(ItemType).GetConstructors();
            //         foreach (ConstructorInfo constructor in ctors)
            //         {
            //             if (constructor.GetParameters().Length == 0)
            //             {
            //                 itemType = (ItemType)constructor.Invoke(null, null);
            //                 ISaveable.ConsumeInput(item);

            //                 break;
            //             }
            //         }
            //         Debug.Log("Using ConsumeInput");
            //         return itemType;
            //     }).ToList();
        }
    }
}
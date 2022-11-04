using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class RoomBlocks
    {
        public List<RoomCells> blocks { get; private set; } = new List<RoomCells>();

        public int Count { get { return blocks.Count; } }

        public void Add(RoomBlocks otherBlocks)
        {
            blocks = blocks.Concat(otherBlocks.blocks).ToList();
        }

        public void Add(RoomCells blockCells)
        {
            blocks.Add(blockCells);
        }

        public void Remove(RoomCells block)
        {
            blocks.RemoveAll(otherBlock => otherBlock == block);
        }
    }
}

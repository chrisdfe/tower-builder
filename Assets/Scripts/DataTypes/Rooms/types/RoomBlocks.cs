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

        public RoomBlocks() { }

        public RoomBlocks(List<RoomCells> blocks)
        {
            this.blocks = blocks;
        }

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

        public void Set(RoomCells blockCells)
        {
            blocks = new List<RoomCells> { blockCells };
        }

        public bool ContainsBlock(RoomCells roomBlock)
        {
            foreach (RoomCells block in blocks)
            {
                if (block == roomBlock)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

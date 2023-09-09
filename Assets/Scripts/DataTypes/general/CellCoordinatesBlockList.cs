using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TowerBuilder.Systems;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CellCoordinatesBlockList : SaveableListWrapper<CellCoordinatesBlock, CellCoordinatesBlock.Input>
    {
        public new class Input : SaveableInputBase
        {
            public List<CellCoordinatesBlock.Input> items;

            public Input() : base() { }
        }

        public CellCoordinatesBlockList() : base() { }
        public CellCoordinatesBlockList(CellCoordinatesBlock cellCoordinatesBlock) : base(cellCoordinatesBlock) { }
        public CellCoordinatesBlockList(List<CellCoordinatesBlock> cellCoordinatesBlocks) : base(cellCoordinatesBlocks) { }
        public CellCoordinatesBlockList(CellCoordinatesBlockList cellCoordinatesBlockList) : base(cellCoordinatesBlockList) { }
        public CellCoordinatesBlockList(Input input) : base()
        {
            ConsumeInput(input);
        }
    }
}

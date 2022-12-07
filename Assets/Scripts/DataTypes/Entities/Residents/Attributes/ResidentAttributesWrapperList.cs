using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Residents.Attributes
{
    public class ResidentAttributesWrapperList : ListWrapper<ResidentAttributesWrapper>
    {
        public ResidentAttributesWrapperList() : base() { }
        public ResidentAttributesWrapperList(ResidentAttributesWrapper ResidentAttributesWrapper) : base(ResidentAttributesWrapper) { }
        public ResidentAttributesWrapperList(List<ResidentAttributesWrapper> ResidentAttributesWrapper) : base(ResidentAttributesWrapper) { }
        public ResidentAttributesWrapperList(ResidentAttributesWrapperList ResidentAttributesWrapperList) : base(ResidentAttributesWrapperList) { }

        public ResidentAttributesWrapper FindByResident(Resident resident)
        {
            return items.Find(ResidentAttributesWrapper => ResidentAttributesWrapper.resident == resident);
        }
    }
}
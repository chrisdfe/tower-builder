using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Journeys
{
    public class JourneyList : ListWrapper<Journey>
    {
        public JourneyList() : base() { }
        public JourneyList(Journey journey) : base(journey) { }
        public JourneyList(List<Journey> journeys) : base(journeys) { }
        public JourneyList(JourneyList journeyList) : base(journeyList) { }
    }
}
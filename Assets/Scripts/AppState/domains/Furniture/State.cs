using System;

namespace TowerBuilder.State.Furnitures
{
    [Serializable]
    public class State
    {
        public struct Input
        {

        }

        public State(Input input) { }

        public State() : this(new Input()) { }
    }
}

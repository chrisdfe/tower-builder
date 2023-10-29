using System;

namespace TowerBuilder.DataTypes.Routes
{
    [Serializable]
    public class RouteException : Exception
    {
        public RouteException() { }

        public RouteException(string message)
            : base(message) { }

        public RouteException(string message, Exception inner)
            : base(message, inner) { }
    }
}
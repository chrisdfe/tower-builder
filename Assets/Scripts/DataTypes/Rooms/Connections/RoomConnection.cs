using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.DataTypes.TransportationItems;

namespace TowerBuilder.DataTypes.Rooms.Connections
{
    public class RoomConnection
    {
        public class Node
        {
            public Room room;
            public TransportationItem transportationItem;
            public TransportationItem.Node transportationItemNode;
            public CellCoordinates cellCoordinates;

            public override string ToString()
            {
                // return $"room: {room.id}, roomEntrance: {roomEntrance}, {roomEntrance.cellCoordinates}";
                return $"room: {room.id}, cellCoordinates: {cellCoordinates}";
            }

            // public Node(Room room, RoomEntrance roomEntrance)
            public Node()
            {
                // this.room = room;
                // this.roomEntrance = roomEntrance;
            }

            public bool Matches(Node otherNode)
            {
                // return room == otherNode.room && roomEntrance == otherNode.roomEntrance;
                return (
                    otherNode.room == room &&
                    otherNode.transportationItem == transportationItem &&
                    otherNode.transportationItemNode == transportationItemNode &&
                    otherNode.cellCoordinates == cellCoordinates
                );
            }
        }

        public Node nodeA;
        public Node nodeB;

        public RoomConnection(Node nodeA, Node nodeB)
        {
            this.nodeA = nodeA;
            this.nodeB = nodeB;
        }

        public override string ToString()
        {
            return $"RoomConnection between {nodeA.room} and {nodeB.room}";
        }

        public bool ContainsRooms(Room roomA, Room roomB)
        {
            return ContainsRoom(roomA) && ContainsRoom(roomB);
        }

        public bool ContainsRoom(Room room)
        {
            return room == nodeA.room || room == nodeB.room;
        }

        // public bool ContainsRoomEntrance(RoomEntrance roomEntrance)
        // {
        //     return roomEntrance == nodeA.roomEntrance || roomEntrance == nodeB.roomEntrance;
        // }

        public Node GetConnectionFor(Room room)
        {
            if (nodeA.room == room)
            {
                return nodeA;
            }

            if (nodeB.room == room)
            {
                return nodeB;
            }

            return null;
        }

        public Node GetOtherConnectionNodeFor(Room room)
        {
            if (nodeA.room == room)
            {
                return nodeB;
            }

            if (nodeB.room == room)
            {
                return nodeA;
            }

            return null;
        }

        public Room GetConnectedRoom(Room room)
        {
            Node node = GetOtherConnectionNodeFor(room);

            if (node != null)
            {
                return node.room;
            }

            return null;
        }
    }
}



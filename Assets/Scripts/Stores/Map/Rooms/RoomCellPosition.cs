
namespace TowerBuilder.Stores.Map.Rooms
{
    // These can be combined to create corners,
    // e.g {Top, Left} or {Bottom,Right}
    // or tunnels
    // e.g {Top, Bottom} or {Left, Right}
    // or tunnel end points
    // e.g {Top, Left, Right}
    public enum RoomCellPosition
    {
        Top,
        Right,
        Bottom,
        Left,
    }
}

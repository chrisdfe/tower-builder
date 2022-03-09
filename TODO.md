# TODO
- CellCoordinates validation stuff should go somewhere other than inside of RoomBlueprintCell
- Build routes from 0,0 to each room
- Flexible-sized rooms should be resizable once placed
- Flexible-sized rooms currently only work in increments of 1x1: make them work in increments of any rectangular room shape.
  - e.g I want to make a wide elevator that takes up 2 horizontal cells
- AND/OR placing a tile next to another flexible room should add onto it instead of creating a new room. i.e for elevators/lobbies
  - perhaps not for XY flexible rooms, like parks?
- Room-specific validations
  - Lobby should be allowed on certain floors only
  - Elevators should not be allowed within a certain distance to each other
- destroy validation
- Map/toolStateHandlers + MapUI/subState/ keeps confusing me
- blueprint price indicator for flexible rooms
- Flexible-sized rooms should cost more per tile - right now they're still a flat cost liek Inflexible rooms
- Awkward naming conflict between ToolState + tool sub states
- Rename MapCursor/MapCursorCells to RoomBlueprintCursor/RoomBlueprintCursorCells
- Moving camera around with middle mouse button
- Move Rooms dictionary in Rooms state somewhere else & make it more extensible
- Generate rooms buttons in RoomBlueprintButtonsManager dynamically
- Mesh for rooms with left, middle, right etc. segment tiles
- transport/rooms connected to transport system
- Add residents
- resident pathfinding
- "Path" constants for paths used in Resource.Load - refactoring/moving things around would be easier
  if they're all in one place

# maybe TODO
- should the destroy tool replace the room with an empty floor, at least on floors 0 and above? otherwise, certain rooms would be undestroyable?

# Done
- Inspect ToolState mode
- Buttons that show active state
- Destroy ToolState mode
- Add click-and-drag room building
- Add flexble-sized rooms
- Remove 'RoomStore' - having just MapStore should be fine
    - Combine RoomDetails and RoomDetails
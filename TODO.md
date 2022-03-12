# TODO

- blueprint price indicator for flexible rooms
- RoomEntrances should live on RoomCell, assigned by Room during initialization based on roomdetails
  - This should help with figuring out where these should live on flexible rooms?
- Build routes from 0,0 to each room
- destroy validation
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
- Z-index constants
- Convert TILE_SIZE to a Vector2
- Flexible-sized rooms should be resizable once placed

# Bugs

- Input.GetMouseButtonDown(0) does not work consistently on macos
- UI is way too big on my laptop

# maybe TODO

- should the destroy tool replace the room with an empty floor, at least on floors 0 and above? otherwise, certain rooms would be undestroyable?

# Done

- Flexible-sized rooms should cost more per tile - right now they're still a flat cost liek Inflexible rooms
- Map/toolStateHandlers + MapUI/subState/ keeps confusing me
- Placing a tile next to another flexible room should add onto it instead of creating a new room. i.e for elevators/lobbies
- Flexible-sized rooms currently only work in increments of 1x1: make them work in increments of any rectangular room shape.
  - e.g I want to make a wide elevator that takes up 2 horizontal cells
- CellCoordinates validation stuff should go somewhere other than inside of RoomBlueprintCell
- RoomCell class (wrapps cellCoordinates)
- Inspect ToolState mode
- Buttons that show active state
- Destroy ToolState mode
- Add click-and-drag room building
- Add flexble-sized rooms
- Remove 'RoomStore' - having just MapStore should be fine
  - Combine RoomDetails and RoomDetails
- Room-specific validations
  - Lobby should be allowed on certain floors only
  - Elevators should not be allowed within a certain distance to each other
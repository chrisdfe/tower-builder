# TODO

- Stop destroying/recreating roomCells whenever the cursor changes position - only when it needs resizing
- Add GameWorldRoomList 
- Elevator entrances should only be at the top and bottom - maybe get ElevatorModule to do this?
- Similar thing with hallways and lobbies
- You should be able to delete a single tile of a flexible-sized room
  - 
- Generate rooms buttons in RoomBlueprintButtonsManager dynamically
- Room types in BuildToolPanel should be grouped by RoomCategory
- for when I implement elevators: Elevator cars can go only from the top to the bottom - no stopping at inbetween places.
  - This helps rationalize having to build all those hallways and stairs
  - Perhaps "upgrades" that can make them stop at 1 other floor per upgrade?
- blueprint price indicator for flexible rooms
- RoomEntrances should live on RoomCell, assigned by Room during initialization based on roomdetails
  - This should help with figuring out where these should live on flexible rooms?
- Build routes from 0,0 to each room
- destroy validation
- Moving camera around with middle mouse button
- Move Rooms dictionary in Rooms state somewhere else & make it more extensible
- Mesh for rooms with left, middle, right etc. segment tiles
- transport/rooms connected to transport system
- Add residents
- resident pathfinding
- "Path" constants for paths used in Resource.Load - refactoring/moving things around would be easier
  if they're all in one place
- Z-index constants
- Convert TILE_SIZE to a Vector2
- Flexible-sized rooms should be resizable once placed

# Cleanup

- things are feeling kind of messy because roomCells are having to know way too much about their room - I should make things more unidirectional
- RoomCells -> RoomCellList OR RoomList -> Rooms
- Standardize around "Initialize/Deinitialize" or "Setup/Teardown"
- Blueprint validators should probably take in the entire Store object
- FloorPlane is confusingly named - it is actually just the collider that watches for the current mouse position, not the floor
- RoomCells could implememnt IEnumerable
- Having RoomUse, RoomCategory AND RoomModules seems like too much?
- A "UI settings" object I can tweak a bunch of stuff in the unity editor with, instead of public serializable fields on each script?
- Awkward naming conflict between ToolState + tool sub states

# Bugs

- Rooms should stay white until they're not being inspected anymore - right now they return to default color on mouse out
- Input.GetMouseButtonDown(0) does not work consistently on macos
- UI is way too big on my laptop

# maybe TODO

- should the destroy tool replace the room with an empty floor, at least on floors 0 and above? otherwise, certain rooms would be undestroyable?

# Done

- Combining rooms seems to delete them right now
- GameWorldBlueprintCell should wrap GameWorldRoomCell just like blueprintCell -> roomCell does
- Room "Modules", to attach to rooms and give them behaviors. Like unity components
- Rename MapCursor/MapCursorCells to RoomBlueprintCursor/RoomBlueprintCursorCells
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
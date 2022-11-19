# TODO

## Current

## Tasks

- Resident validation
- Furniture validation
- Connections to outside
  - RouteSegmentNode subtype for outside connections
  - RoomConnection subtype for outside connections
  - Wheels room should allow ladder furniture
  - Residents should be able to walk on the ground and climb up the wheels ladder
- Convert other List wrappers to use the new ListWrapper generic
- Rooms aren't combining correctly anymore
- "reset" button to reset state to default
- Replace ground cells with a simpler ground for now
- ability to start game with a non-empty state already - load rooms, connections, residents, current time, etc
- colors should be tied to room category + not live on RoomTemplate
- RouteFinder shouldn't look for every room entrance in the room, just on the current floor - it should also look for furniture on the current floor that could transport the resident elsewhere
- Transportation room furniture interface - have a "connects to"
  - Room entrances could be "Doorway" furniture instead
- Furniture "usage slots" - an array of residents currently using this piece of furniture the size of the furnitures occupancy
- roomcell "has floor" boolean state - for tall rooms
  - additionally, a "bridge" furniture type?
- Residents currently stay on the same cell for more than one cycle currently - because of repeating cellCoordinates in segment end/start
- Wallet transation history
  - Wallet "batch" transactions so the transactions don't get flooded with lots of tiny transactions (e.g. desk income)
    - transactions will get added to queue, grouped by string id/message, then OnTick() they get applied
- Resident furniture behaviors
  - furniture "controls" resident while they're using it
- Room furniture
  - beds
  - desks
  - stairs??
  - elevator cars???
    - potentially furniture could replace "room modules"
      - rooms would just be generic rooms, and furniture would dictate the behavior/functionality
- "Schedules" store, possibly right below timestore? residents will have schedules, but potentially weather effects and other such things could use this schedule mechanic
- Route weighting mechanism to figure out which route is the best to take
  - probably as simple as fewer cells traveled > more cells traveled
- Keybindings for build, destroy, inspect, none
- 'closed' connections, e.g between 2 private rooms (e.g condo)
- Stairwells should not be able to be placed next to elevators or other stairwells
  - Perhaps this should be part of "Transportation category validation"
- Resizing rooms should also reset their entrances (e.g if an elevator had a connection but has been resized)
- Stop destroying/recreating roomCells whenever the cursor changes position - only when it needs resizing
- destroy validation
- rooms should not be able to be built in thin air above ground floor
- "Path" constants for paths used in Resource.Load - refactoring/moving things around would be easier
  if they're all in one place
- Z-index constants
- Convert TILE_SIZE to a Vector2
- should the destroy tool replace the room with an empty floor, at least on floors 0 and above? otherwise, certain rooms would be undestroyable?

## Projects

- turn this into a car game
- Kitchen/restaurant room
- Top-level "KeyBindingsManager" that listens for user input + dispatches actions
- UI overlays
- Dynamic weather
- save/load

## Cleanup

- Standardize around "Initialize/Deinitialize" or "Setup/Teardown"
- RouteFinder creates too many branches
- RoomCells -> RoomCellList OR RoomList -> Rooms
- FloorPlane is confusingly named - it is actually just the collider that watches for the current mouse position, not the floor
- ListWrapper should implememnt IEnumerable
- A "UI settings" object I can tweak a bunch of stuff in the unity editor with, instead of public serializable fields on each script?
- Awkward naming conflict between ToolState + tool sub states

## Ideas

- this game loop:
  - Vehicles (moving vehicles) need fuel
  - You need scrap (currency) to buy fuel
  - You get scrap by traveling and looting/farming/gathering
  - You also need residents using the "engine" furniture
- Periodic supplies delivery? big old quarry trucks, helicopers, pack mule, etc

# Done

- consider putting vehicles state further down in the heirarchy - having vehicles be able to query their contents in various ways
  (e.g how many residents are currently inside of it, or whether it has a cockpit/engine) would be helpful
- Delete room entrances when a block gets deleted as well
- Move current route traversal Resident code out into separate "motor" class
- Ability to destroy a single block in a flexible room
- Move Rooms dictionary in Rooms state somewhere else & make it more extensible
- blueprint price indicator for flexible rooms
- Add GameWorldRoomList
- You should be able to delete a single block of a flexible-sized room
- Add residents
- namespace reorganization
  - Split "type definitions (Room, Route, Resident etc) into "Data" namespace
  - Rename "Stores" to "GameState" or something
- When casting the mouse ray, build a stack of interactable/inspectable elements
  - If that stack contains a ui element:
    - on mouse down don't propagate
    - on mouse up, still propagate but handle it further up the chain
  - Eventually this will allow for multiple clicks in the same location cycling through this list
- Add concept of different "vehicles" or separate entities - groups of rooms. When you add/delete a room it can either add to an existing vehicle or create a new one
- Blueprint should just be a room in RoomState - right now it lives in a seperate place in BuildToolState;
- Draggable destroy tool
- Move current RoomDefinitions constants to a field in Registry - with ability to split up the "registering room template" logic into different files
- Moving camera around with middle mouse button
- Camera zoom in/out
- Room addition is broken again
- Generate rooms buttons in RoomBlueprintButtonsManager dynamically
  - Room types in BuildToolPanel should be grouped by RoomCategory
- Flexible-sized rooms should remember the 'blocks' they are made up of, to avoid having to recalculate it when you destroy individual blocks.
- Blueprint validators should probably take in the entire Store object
- Rename RoomDetails to RoomTemplate
- Use factories for RoomValidator and RoomEntranceBuilder, right now all rooms of the same type share the same instance
- GetRoomPrice should live on Room, not Blueprint
- Debug resident walk along route to destination
- Perhaps Stores.Map.Rooms should just be Stores.Rooms and delete Stores.Map
  - Also rename "MapUI" to just "UI"
- RoomEntrances namespace?
- Make ground a cube, cut holes in it when you build a room
- RoomCellValidators that follow the same pattern as EntranceBuilders instead of what I have now
- resident pathfinding
- For debug purposes - draw line between cells, along route
- transport/rooms connected to transport system
- "RoomCellPosition" is confusing - change to "RoomCellOrientation"
- Having RoomUse, RoomCategory AND RoomModules seems like too much?
- Rename 'useDetails' to 'moduleDetails'
- for when I implement elevators: Elevator cars can go only from the top to the bottom - no stopping at inbetween places.
  - This helps rationalize having to build all those hallways and stairs
  - Perhaps "upgrades" that can make them stop at 1 other floor per upgrade?
- things are feeling kind of messy because roomCells are having to know way too much about their room - I should make things more unidirectional
- Rooms should stay white until they're not being inspected anymore - right now they return to default color on mouse out
- Removing rooms should also delete their room connections
- Add blueprint room connections to main room connections list in MapStore when the room gets built
- Elevator entrances should only be at the top and bottom - maybe get ElevatorModule to do this?
  - Similar thing with hallways and lobbies
- Mesh for rooms with left, middle, right etc. segment tiles
- Combining rooms seems to delete them right now
- GameWorldBlueprintCell should wrap GameWorldRoomCell just like blueprintCell -> roomCell does
- Room "Modules", to attach to rooms and give them behaviors. Like unity components
- Rename MapCursor/MapCursorCells to RoomBlueprintCursor/RoomBlueprintCursorCells
- Flexible-sized rooms should cost more per tile - right now they're still a flat cost liek Inflexible rooms
- Map/toolStateHandlers + UI/subState/ keeps confusing me
- Placing a tile next to another flexible room should add onto it instead of creating a new room. i.e for elevators/lobbies
- Flexible-sized rooms currently only work in increments of 1x1: make them work in increments of any rectangular room shape.
  - e.g I want to make a wide elevator that takes up 2 horizontal cells
- CellCoordinates validation stuff should go somewhere other than inside of RoomBlueprintCell
- RoomCell class (wrapps cellCoordinates)
- Inspect ToolState mode
- Buttons that show active state
- Destroy ToolState mode
- Add click-and-drag room vehicle
- Add flexble-sized rooms
- Remove 'RoomStore' - having just MapStore should be fine
  - Combine RoomTemplate and RoomTemplate
- Room-specific validations
  - Lobby should be allowed on certain floors only
  - Elevators should not be allowed within a certain distance to each other

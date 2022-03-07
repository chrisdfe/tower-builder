# TODO
- Flexible-sized rooms should cost more per tile - right now they're still a flat cost liek Inflexible rooms
- Inspect ToolState mode
- Awkward naming conflict between ToolState + tool sub states
- Buttons that show active state
- Rename MapCursor/MapCursorCells to RoomBlueprintCursor/RoomBlueprintCursorCells
- Moving camera around with middle mouse button
- Move Rooms dictionary in Rooms state somewhere else & make it more extensible
- Generate rooms buttons in RoomBlueprintButtonsManager dynamically
- Mesh for rooms with left, middle, right etc. segment tiles
- Add residents
- "Path" constants for paths used in Resource.Load - refactoring/moving things around would be easier
  if they're all in one place

# Done
- Destroy ToolState mode
- Add click-and-drag room building
- Add flexble-sized rooms
- Remove 'RoomStore' - having just MapStore should be fine
    - Combine RoomDetails and MapRoomDetails
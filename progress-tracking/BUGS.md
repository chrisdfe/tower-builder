# Bugs

## Current

## TODO

- Ladders aren't tiling properly
- Residents don't seem to be interacting with furniture properly right now
- Sometimes when sending a resident back and forth between cells there's an out of bounds exception with RouteProgress
- Figure out why residents stay in one spot for a while when traveling
- Entity doesn't update when an entity is built
- Rooms aren't combining correctly anymore
- Individual roomCells should know if they are valid again (right now it's just the room)
- Fix that NullReferenceArea in MapManager that shows up when defocusing/refocusing on the window again
- RoomEntrances in the blueprint room aren't getting highlighted
- Input.GetMouseButtonDown(0) does not work consistently on macos
- UI is way too big on my laptop

# Done

- Room connections don't work reliably (probably because of room blocks)
- Inspect highlight doesn't stay after user mouses out
- Rooms + vehicles still don't get destroyed properly
- Sometimes room entrances don't get deleted
- Clicking on buttons in the UI also clicks on what ever is behind it, e.g. vehicle a room when you meant to click on the button

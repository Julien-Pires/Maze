namespace Maze.Engine.Tests

open Maze.Engine

[<AutoOpen>]
module Fixtures =
    let noRooms : Room[][] = [|
        [| Void; Void; Void |]
        [| Void; Void; Void |]
        [| Void; Void; Void |]|]

    let partialRooms : Room[][] = [|
        [| Void; Empty; Void |]
        [| Empty; Empty; Empty |]
        [| Void; Empty; Void |]|]

    let fullRooms : Room[][] = [|
        [| Empty; Empty; Empty |]
        [| Empty; Empty; Empty |]
        [| Empty; Empty; Empty |]|]

module Direction =
    let up = "up"
    let down = "down"
    let left = "left"
    let right = "right"
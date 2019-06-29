namespace Maze.Engine.Tests

open Maze.Engine

[<AutoOpen>]
module Fixtures =
    let noRooms = []

    let withRooms =
        [ ({ X = 0; Y = 0}, { Type = Normal })
          ({ X = 1; Y = 0}, { Type = Normal })
          ({ X = 2; Y = 0}, { Type = Normal })
          ({ X = 0; Y = 1}, { Type = Normal })
          ({ X = 1; Y = 1}, { Type = Normal })
          ({ X = 2; Y = 1}, { Type = Normal })
          ({ X = 0; Y = 2}, { Type = Normal })
          ({ X = 1; Y = 2}, { Type = Normal })
          ({ X = 2; Y = 2}, { Type = Normal }) ]
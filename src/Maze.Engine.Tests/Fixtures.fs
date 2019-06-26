namespace Maze.Engine.Tests

open Maze.Engine

[<AutoOpen>]
module Fixtures =
    let noRooms = []

    let partialRooms =
        [ ({ X = 1; Y = 0}, Empty)
          ({ X = 0; Y = 1}, Empty)
          ({ X = 1; Y = 1}, Empty)
          ({ X = 2; Y = 1}, Empty)
          ({ X = 1; Y = 2}, Empty) ]

    let fullRooms =
        [ ({ X = 0; Y = 0}, Empty)
          ({ X = 1; Y = 0}, Empty)
          ({ X = 2; Y = 0}, Empty)
          ({ X = 0; Y = 1}, Empty)
          ({ X = 1; Y = 1}, Empty)
          ({ X = 2; Y = 1}, Empty)
          ({ X = 0; Y = 2}, Empty)
          ({ X = 1; Y = 2}, Empty)
          ({ X = 2; Y = 2}, Empty) ]
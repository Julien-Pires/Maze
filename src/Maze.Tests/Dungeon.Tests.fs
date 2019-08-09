namespace Maze.Tests

open Expecto
open Swensen.Unquote
open Maze

module Dungeon_Tests =
    [<Tests>]
    let moveTests =
        let emptyMap = 
            { Rooms = Map(noRooms) }
        let fullMap =
            { Rooms = Map(withRooms) }
        testList "Dungeon/move" [
            yield!
                testTheory "should not move position when there is no room at target position"
                [ ({ X = 1; Y = 1 }, Forward)
                  ({ X = 1; Y = 1 }, Backward)
                  ({ X = 1; Y = 1 }, Left)
                  ({ X = 1; Y = 1 }, Right) ] <|
                fun (position, direction) () ->
                    test <@ Dungeon.move direction position emptyMap
                            |> fun c -> c.Value = position @>
             
            yield!
                testTheory "should move position when there is a room at target position"
                [ ({ X = 1; Y = 1 }, { X = 1; Y = 2 }, Forward)
                  ({ X = 1; Y = 1 }, { X = 1; Y = 0 }, Backward)
                  ({ X = 1; Y = 1 }, { X = 0; Y = 1 }, Left)
                  ({ X = 1; Y = 1 }, { X = 2; Y = 1 }, Right) ] <|
                fun (position, target, direction) () ->
                    test <@ Dungeon.move direction position fullMap
                            |> fun c -> c.Value = target @> ]
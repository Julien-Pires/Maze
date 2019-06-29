namespace Maze.Engine.Tests

open Expecto
open Swensen.Unquote
open Maze.Engine

module Map_Tests =
    [<Tests>]
    let canMoveTests =
        let emptyMap =
            { Rooms = Map(noRooms) }
        let fullMap =
            { Rooms = Map(withRooms) }

        testList "Map/CanMove" [
            yield
                testCase "should return true when the target is identical to the original position"
                (fun _ ->
                    test <@ emptyMap |> Map.canMove { X = 0; Y = 0 } { X = 0; Y = 0 } @> )

            yield!
                testTheory "should return true when there is a room at the specified position"
                [ ({ X = 1; Y = 1}, { X = 0; Y = 1 })
                  ({ X = 1; Y = 1}, { X = 2; Y = 1 })
                  ({ X = 1; Y = 1}, { X = 1; Y = 0 })
                  ({ X = 1; Y = 1}, { X = 1; Y = 2 }) ]
                (fun (position, target) () ->
                    test <@ fullMap |> Map.canMove position target @> )
                    
            yield!
                testTheory "should return false when there is no room at the specified position"
                [ ({ X = 2; Y = 0}, { X = 3; Y = 0 }) 
                  ({ X = 0; Y = 0}, { X = -1; Y = 0 })
                  ({ X = 0; Y = 2}, { X = 0; Y = 3 })
                  ({ X = 0; Y = 0}, { X = 0; Y = -1 }) ]
                (fun (position, target) () ->
                    test <@ fullMap |> Map.canMove position target |> not @> )
                    
            yield!
                testTheory "should return false when the move is in diagonal"
                [ ({ X = 1; Y = 1}, { X = 0; Y = 0 }) 
                  ({ X = 1; Y = 1}, { X = 0; Y = 2 })
                  ({ X = 1; Y = 1}, { X = 2; Y = 0 })
                  ({ X = 1; Y = 1}, { X = 2; Y = 2 }) ]
                (fun (position, target) () ->
                    test <@ fullMap |> Map.canMove position target |> not @> )
                    
            yield!
                testTheory "should return false when the move is more than one room away"
                [ ({ X = 0; Y = 0}, { X = 2; Y = 0 }) 
                  ({ X = 0; Y = 0}, { X = 0; Y = 2 })
                  ({ X = 2; Y = 2}, { X = 2; Y = 0 })
                  ({ X = 2; Y = 2}, { X = 0; Y = 2 }) ]
                (fun (position, target) () ->
                    test <@ fullMap |> Map.canMove position target |> not @> ) ]

    [<Tests>]
    let hasReachedExitTests =
        let map =
            { Rooms = Map(withRooms) }

        testList "Map/hasReachedExit" [
            yield
                testCase "should return true when the room at specified position is an entrance"
                    (fun _ ->
                        let mapWithExit =  { map with Rooms = map.Rooms.Add ({ X = 2; Y = 2 }, { Type = Entrance }) }

                        test <@ mapWithExit |> Map.hasReachedExit { X = 2; Y = 2 } @>) 
            
            yield
                testCase "should return false when the room at specified position is not an entrance"
                    (fun _ ->
                        test <@ map |> Map.hasReachedExit { X = 0; Y = 0 } |> not @>)]
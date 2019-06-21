namespace Maze.Engine.Tests

open Expecto
open Swensen.Unquote
open Maze.Engine

module Map_Tests =
    [<Tests>]
    let canMoveTests =
        testList "Map/CanMove" [
            yield 
                testCase "should return true when the target is identical to the original position"
                (fun _ ->
                    let map = { Rooms = Map(noRooms) }
            
                    test <@ map |> Map.canMove { X = 0; Y = 0 } { X = 0; Y = 0 } @> )

            yield!
                testTheory "should return true when there is a room at the specified position"
                [ ({ X = 1; Y = 1}, { X = 0; Y = 1 }) 
                  ({ X = 1; Y = 1}, { X = 2; Y = 1 })
                  ({ X = 1; Y = 1}, { X = 1; Y = 0 })
                  ({ X = 1; Y = 1}, { X = 1; Y = 2 }) ]
                (fun (position, target) () ->
                    let map = { Rooms = Map(fullRooms) }
                    
                    test <@ map |> Map.canMove position target @> )
                    
            yield!
                testTheory "should return false when there is no room at the specified position"
                [ ({ X = 1; Y = 0}, { X = 0; Y = 0 }) 
                  ({ X = 1; Y = 0}, { X = 2; Y = 0 })
                  ({ X = 0; Y = 1}, { X = 0; Y = 0 })
                  ({ X = 0; Y = 1}, { X = 0; Y = 2 }) ]
                (fun (position, target) () ->
                    let map = { Rooms = Map(partialRooms) }
                
                    test <@ map |> Map.canMove position target |> not @> )
                    
            yield!
                testTheory "should return false when the move is in diagonal"
                [ ({ X = 1; Y = 1}, { X = 0; Y = 0 }) 
                  ({ X = 1; Y = 1}, { X = 0; Y = 2 })
                  ({ X = 1; Y = 1}, { X = 2; Y = 0 })
                  ({ X = 1; Y = 1}, { X = 2; Y = 2 }) ]
                (fun (position, target) () ->
                    let map = { Rooms = Map(fullRooms) }
                    
                    test <@ map |> Map.canMove position target |> not @> )
                    
            yield!
                testTheory "should return false when the move is more than one room away"
                [ ({ X = 0; Y = 0}, { X = 2; Y = 0 }) 
                  ({ X = 0; Y = 0}, { X = 0; Y = 2 })
                  ({ X = 2; Y = 2}, { X = 2; Y = 0 })
                  ({ X = 2; Y = 2}, { X = 0; Y = 2 }) ]
                (fun (position, target) () ->
                    let map = { Rooms = Map(fullRooms) }
                
                    test <@ map |> Map.canMove position target |> not @> ) ]
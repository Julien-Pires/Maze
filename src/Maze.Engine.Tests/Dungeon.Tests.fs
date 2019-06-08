namespace Maze.Engine.Tests

open Expecto
open Swensen.Unquote
open Maze.Engine

module Dungeon =
    let emptyDungeon : Room[][] = [|
        [| Void; Void; Void |]
        [| Void; Void; Void |]
        [| Void; Void; Void |]|]

    let partialDungeon : Room[][] = [|
        [| Void; Empty; Void |]
        [| Empty; Empty; Empty |]
        [| Void; Empty; Void |]|]

    let fullDungeon : Room[][] = [|
        [| Empty; Empty; Empty |]
        [| Empty; Empty; Empty |]
        [| Empty; Empty; Empty |]|]

    [<Tests>]
    let canMoveTests =
        testList "Dungeon CanMove" [
            yield 
                testCase "should return true when the target is identical to the original position"
                (fun _ ->
                    let sut = Dungeon({ Rooms = emptyDungeon })
            
                    test <@ sut.CanMove { X = 0; Y = 0 } { X = 0; Y = 0 } @> )

            yield!
                testTheory "should return true when there is a room at the specified position"
                [ ({ X = 1; Y = 1}, { X = 0; Y = 1 }) 
                  ({ X = 1; Y = 1}, { X = 2; Y = 1 })
                  ({ X = 1; Y = 1}, { X = 1; Y = 0 })
                  ({ X = 1; Y = 1}, { X = 1; Y = 2 }) ]
                (fun (position, target) () ->
                    let sut = Dungeon({ Rooms = fullDungeon })
                    
                    test <@ sut.CanMove position target @> )
                    
            yield!
                testTheory "should return false when there is no room at the specified position"
                [ ({ X = 1; Y = 0}, { X = 0; Y = 0 }) 
                  ({ X = 1; Y = 0}, { X = 2; Y = 0 })
                  ({ X = 0; Y = 1}, { X = 0; Y = 0 })
                  ({ X = 0; Y = 1}, { X = 0; Y = 2 }) ]
                (fun (position, target) () ->
                    let sut = Dungeon({ Rooms = partialDungeon })
                
                    test <@ sut.CanMove position target |> not @> )
                    
            yield!
                testTheory "should return false when the move is in diagonal"
                [ ({ X = 1; Y = 1}, { X = 0; Y = 0 }) 
                  ({ X = 1; Y = 1}, { X = 0; Y = 2 })
                  ({ X = 1; Y = 1}, { X = 2; Y = 0 })
                  ({ X = 1; Y = 1}, { X = 2; Y = 2 }) ]
                (fun (position, target) () ->
                    let sut = Dungeon({ Rooms = fullDungeon })
                    
                    test <@ sut.CanMove position target |> not @> )
                    
            yield!
                testTheory "should return false when the move is more than one room away"
                [ ({ X = 0; Y = 0}, { X = 2; Y = 0 }) 
                  ({ X = 0; Y = 0}, { X = 0; Y = 2 })
                  ({ X = 2; Y = 2}, { X = 2; Y = 0 })
                  ({ X = 2; Y = 2}, { X = 0; Y = 2 }) ]
                (fun (position, target) () ->
                    let sut = Dungeon({ Rooms = fullDungeon })
                
                    test <@ sut.CanMove position target |> not @> )
                    
            yield!
                testTheory "should return false when the move is outside the map"
                [ ({ X = 0; Y = 0}, { X = -1; Y = 0 }) 
                  ({ X = 0; Y = 0}, { X = 0; Y = -1 })
                  ({ X = 2; Y = 2}, { X = 3; Y = 2 })
                  ({ X = 2; Y = 2}, { X = 2; Y = 3 }) ]
                (fun (position, target) () ->
                    let sut = Dungeon({ Rooms = fullDungeon })
            
                    test <@ sut.CanMove position target |> not @> ) ]
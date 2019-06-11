namespace Maze.Engine.Tests

open Expecto
open Swensen.Unquote
open Maze.Engine

module Board_Tests =
    [<Tests>]
    let moveTests =
        testList "Board/Do with Move action" [
            yield!
                testTheory "should not move character position when there is no room"
                [ ({ X = 1; Y = 1 }, { X = 1; Y = 1 }, Direction.up)
                  ({ X = 1; Y = 1 }, { X = 1; Y = 1 }, Direction.down)
                  ({ X = 1; Y = 1 }, { X = 1; Y = 1 }, Direction.left)
                  ({ X = 1; Y = 1 }, { X = 1; Y = 1 }, Direction.right) ]
                ( fun (position, target, direction) () ->
                    let character = { Name = "Adventurer"; Position = position }
                    let sut = Board(character, Map(noRooms))
                    sut.Do (Move direction) 
                    
                    test <@ sut.GetCharacter() |> fun c -> c.Position = target @> )
             
            yield!
                testTheory "should move character position when there is a room"
                [ ({ X = 1; Y = 1 }, { X = 1; Y = 0 }, Direction.up)
                  ({ X = 1; Y = 1 }, { X = 1; Y = 2 }, Direction.down)
                  ({ X = 1; Y = 1 }, { X = 0; Y = 1 }, Direction.left)
                  ({ X = 1; Y = 1 }, { X = 2; Y = 1 }, Direction.right) ]
                ( fun (position, target, direction) () ->
                    let character = { Name = "Adventurer"; Position = position }
                    let sut = Board(character, Map(fullRooms))
                    sut.Do (Move direction) 
                
                    test <@ sut.GetCharacter() |> fun c -> c.Position = target @> ) ]
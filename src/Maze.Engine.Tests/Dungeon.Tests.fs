namespace Maze.Engine.Tests

open Expecto
open Swensen.Unquote
open Maze.Engine

module Dungeon_Tests =
    [<Tests>]
    let moveTests =
        testList "Dungeon/Move command" [
            yield!
                testTheory "should not move character position when there is no room"
                [ ({ X = 1; Y = 1 }, Forward)
                  ({ X = 1; Y = 1 }, Backward)
                  ({ X = 1; Y = 1 }, Left)
                  ({ X = 1; Y = 1 }, Right) ]
                ( fun (position, direction) () -> 
                    let dungeon =
                      { Character = ({ Name = "Adventurer" }, position)
                        Map = { Rooms = Map noRooms } }
                    
                    test <@ dungeon |> Dungeon.move direction
                                    |> function | Success _ -> false
                                                | Failure _ -> true @> )
             
            yield!
                testTheory "should move character position when there is a room"
                [ ({ X = 1; Y = 1 }, { X = 1; Y = 2 }, Forward)
                  ({ X = 1; Y = 1 }, { X = 1; Y = 0 }, Backward)
                  ({ X = 1; Y = 1 }, { X = 0; Y = 1 }, Left)
                  ({ X = 1; Y = 1 }, { X = 2; Y = 1 }, Right) ]
                ( fun (position, target, direction) () ->
                    let dungeon = 
                      { Character = ({ Name = "Adventurer" }, position)
                        Map = { Rooms = Map fullRooms } }
                
                    test <@ dungeon |> Dungeon.move direction
                                    |> function | Success x -> x.Value.Character |> snd = target
                                                | Failure _ -> false @> ) ]
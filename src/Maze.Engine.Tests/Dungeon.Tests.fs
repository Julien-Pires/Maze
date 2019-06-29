namespace Maze.Engine.Tests

open Expecto
open Swensen.Unquote
open Maze.Engine

module Dungeon_Tests =
    [<Tests>]
    let moveTests =
        let emptyMap = 
            { Rooms = Map(noRooms) }
        let fullMap =
            { Rooms = Map(withRooms) }
        testList "Dungeon/Move command" [
            yield!
                testTheory "should not move character position when there is no room"
                [ ({ X = 1; Y = 1 }, Forward)
                  ({ X = 1; Y = 1 }, Backward)
                  ({ X = 1; Y = 1 }, Left)
                  ({ X = 1; Y = 1 }, Right) ]
                (fun (position, direction) () -> 
                    let dungeon =
                      { Character = ({ Name = "Adventurer" }, position)
                        Map = emptyMap }
                    
                    test <@ dungeon |> Dungeon.move direction
                                    |> function | Ok _ -> false
                                                | Error _ -> true @>)
             
            yield!
                testTheory "should move character position when there is a room"
                [ ({ X = 1; Y = 1 }, { X = 1; Y = 2 }, Forward)
                  ({ X = 1; Y = 1 }, { X = 1; Y = 0 }, Backward)
                  ({ X = 1; Y = 1 }, { X = 0; Y = 1 }, Left)
                  ({ X = 1; Y = 1 }, { X = 2; Y = 1 }, Right) ]
                (fun (position, target, direction) () ->
                    let dungeon = 
                      { Character = ({ Name = "Adventurer" }, position)
                        Map = fullMap }
                
                    test <@ dungeon |> Dungeon.move direction
                                    |> function | Ok x -> x.Value.Character |> snd = target
                                                | Error _ -> false @>) ]
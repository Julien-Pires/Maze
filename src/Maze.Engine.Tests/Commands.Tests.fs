namespace Maze.Engine.Tests

open Expecto
open Swensen.Unquote
open Maze.Engine

module Commands_Tests = 
    [<Tests>]
    let parseTests =
        testList "CommandsParser/parse" [
            yield!
                testTheory "should return an action when input is an action"
                [ ("move left", Move Direction.left) 
                  ("move right", Move Direction.right)
                  ("move up", Move Direction.up)
                  ("move down", Move Direction.down) ]
                (fun (input, expected) () ->
                    test <@ CommandsParser.parse input = Some expected @>) ]
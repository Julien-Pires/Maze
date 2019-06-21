namespace Maze.Engine.Tests

open Expecto
open Swensen.Unquote
open Maze.Engine

module Parser_Tests = 
    [<Tests>]
    let parseTests =
        testList "Parser/parse" [
            yield!
                testTheory "should return an action when input is an action"
                [ ("move Left", Move Left) 
                  ("move Right", Move Right)
                  ("move Forward", Move Forward)
                  ("move Backward", Move Backward) ]
                (fun (input, expected) () ->
                    test <@ CommandsParser.parse input = Some expected @>) ]
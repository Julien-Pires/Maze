namespace Maze.Engine.Tests

open Expecto
open Swensen.Unquote
open Maze.Engine

module Parser_Tests = 
    [<Tests>]
    let parseTests =
        testList "Parser/parse" [
            yield
                testCase "should return none when input is not a valid direction"
                (fun _ ->
                    test <@ CommandsParser.parse "foo" = None @>)

            yield!
                testTheory "should return a move command when input is a valid direction"
                [ ("left", Move Left)
                  ("LEFT", Move Left)
                  ("right", Move Right)
                  ("RIGHT", Move Right)
                  ("forward", Move Forward)
                  ("FORWARD", Move Forward)
                  ("backward", Move Backward)
                  ("BACKWARD", Move Backward) ]
                (fun (input, expected) () ->
                    test <@ CommandsParser.parse input = Some expected @>) ]
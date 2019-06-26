namespace Maze.Engine.Tests

open Expecto

[<AutoOpen>]
module Expecto =
    let testTheory name parameters test = [
        for param in parameters do
            yield testCase (sprintf "%s (Parameters: %A)" name param) (test param) ]
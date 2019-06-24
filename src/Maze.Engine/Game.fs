namespace Maze.Engine

open FSharp.Control

type Input = string -> unit

type Game = {
    Result: AsyncSeq<WorldResponse> }

module Game =
    let init dungeon =
        let results = World.start dungeon
        { Result = results }

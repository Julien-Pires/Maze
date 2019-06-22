namespace Maze.Engine

open System
open FSharp.Control
open Maze.FSharp

type Input = string -> unit

type Game = {
    Input: Input
    Result: AsyncSeq<WorldResult> }

module Game =
    let init dungeon =
        let (worldInput, worldObservable) = World.start dungeon

        { Input = worldInput
          Result = worldObservable }

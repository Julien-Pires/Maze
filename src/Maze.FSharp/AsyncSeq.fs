namespace Maze.FSharp

open FSharp.Control

type AsyncSeqSource<'a>() =
    let a = asyncSeq {
        yield 1
    }
namespace Maze.Engine

open FSharp.Control

type Game = {
    Responses: AsyncSeq<OutputCommand> }

module Game =
    let init dungeon =
        let responsesSeq = World.start dungeon
        { Responses = responsesSeq }

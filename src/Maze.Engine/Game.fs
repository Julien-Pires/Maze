namespace Maze.Engine

open FSharp.Control

type Game = {
    Responses: AsyncSeq<OutputCommand> }

module Game =
    let init map =
        let dungeon = {
            Character = { Name = "Indiana Jones"}, { X = 0; Y = 0 }
            Map = map
        }
        let responsesSeq = World.start dungeon
        { Responses = responsesSeq }

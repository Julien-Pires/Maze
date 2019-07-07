namespace Maze.Engine

open FSharp.Data
open Maze.FSharp

type DungeonProvider = JsonProvider<"Templates/Dungeon_Template.json">

module Data =
    let loadMap (getText: Async<Result<string, string>>) = async {
        let! result = getText
        match result with
        | Ok text ->
            try
                let root = DungeonProvider.Parse(text)
                let rooms =
                    root.Rooms
                    |> Seq.map (fun c ->
                        let roomType = unsafeFromString<RoomType> false c.Type
                        let position = { X = c.Position.X; Y = c.Position.Y }
                        (position, { Type = roomType }) )
                    |> Map.ofSeq
                return Ok { Rooms = rooms }
            with
                | ex -> return Error ex.Message
        | Error err -> return Error err
    }
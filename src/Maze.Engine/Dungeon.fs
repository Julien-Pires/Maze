namespace Maze.Engine

open System

type Room =
    | Void
    | Empty
    | Exit

type Map = {
    Rooms: Room[][] }

type Dungeon(map: Map) =
    let height = map.Rooms.Length
    let width = map.Rooms.[0].Length

    member __.CanMove position target =
        let xDiff = Math.Abs (position.X - target.X)
        let yDiff = Math.Abs (position.Y - target.Y)
        match xDiff + yDiff with
        | 0 -> true
        | x when x > 1 -> false
        | _ ->
            if target.X < 0 || target.X >= width then false
            else if target.Y < 0 || target.Y >= height then false
            else
                match map.Rooms.[target.Y].[target.X] with
                | Void -> false
                | _ -> true
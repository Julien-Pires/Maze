namespace Maze.Engine

open System

type Room =
    | Void
    | Empty
    | Exit

type Rooms = (Position * Room) list

type Map = {
    Rooms: Map<Position, Room> }

module Map =
    let canMove position target map =
        let xDiff = Math.Abs (position.X - target.X)
        let yDiff = Math.Abs (position.Y - target.Y)
        match xDiff + yDiff with
        | 0 -> true
        | x when x > 1 -> false
        | _ ->
            match map.Rooms |> Map.tryFind target with
            | Some Void -> false
            | Some _ -> true
            | _ -> false
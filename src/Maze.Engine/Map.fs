namespace Maze.Engine

open System

type RoomType =
    | Normal
    | Entrance

type Room =
    { Type: RoomType }

type Rooms = Room list

type Map =
    { Rooms: Map<Position, Room> }

module Map =
    let canMove position target map =
        let xDiff = Math.Abs (position.X - target.X)
        let yDiff = Math.Abs (position.Y - target.Y)
        match xDiff + yDiff with
        | 0 -> true
        | x when x > 1 -> false
        | _ ->
            match map.Rooms |> Map.tryFind target with
            | Some _ -> true
            | _ -> false
    
    let hasReachedExit position map =
        match map.Rooms |> Map.tryFind position with
        | Some x -> x.Type = Entrance
        | None -> false
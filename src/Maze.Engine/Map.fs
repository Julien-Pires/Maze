namespace Maze.Engine

open System

type Room =
    | Void
    | Empty
    | Exit

type Rooms = (Position * Room) list

type Map(rooms: Rooms) =
    let rooms = rooms |> Map.ofList

    member __.CanMove position target =
        let xDiff = Math.Abs (position.X - target.X)
        let yDiff = Math.Abs (position.Y - target.Y)
        match xDiff + yDiff with
        | 0 -> true
        | x when x > 1 -> false
        | _ ->
            match rooms |> Map.tryFind target with
            | Some Void -> false
            | Some _ -> true
            | _ -> false
namespace Maze.Engine

type ActionResult<'a> = {
    Value: 'a 
    Message: string }

type Direction = string

module Direction =
    let (|Up|Down|Left|Right|None|) = function
        | "up" -> Up
        | "down" -> Down
        | "left" -> Left
        | "right" -> Right
        | _ -> None

type Action =
    | Move of Direction
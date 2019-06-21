namespace Maze.Engine

open Maze.FSharp

type Agent<'a> = MailboxProcessor<'a>

type Result<'a, 'b> =
    | Success of 'a
    | Failure of 'b

type ActionResult<'a> = {
    Value: 'a 
    Message: string }

type Position = {
    X: int
    Y: int }

type Direction =
    | Forward
    | Backward
    | Left
    | Right
    static member FromString s = fromString<Direction> s
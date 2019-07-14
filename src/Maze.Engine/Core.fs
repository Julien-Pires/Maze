namespace Maze.Engine

type Message = string

type Position = {
    X: int
    Y: int }

type Direction =
    | Forward
    | Backward
    | Left
    | Right
    
type IGameContext =
    abstract member Update : string -> unit
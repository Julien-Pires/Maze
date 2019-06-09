namespace Maze.Engine

type ActionResult<'a> = {
    Value: 'a 
    Message: string }

type Action =
    | Move of Direction
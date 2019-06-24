namespace Maze.Engine

type Message = string

type CommandResult<'a> = {
    Value: 'a
    Message: Message }

type Command =
    | Move of Direction
    | GetPosition of AsyncReplyChannel<Position>
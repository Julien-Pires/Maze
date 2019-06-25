namespace Maze.Engine

type Command =
    | Move of Direction
    | GetPosition of AsyncReplyChannel<Position>

type Message = string

type CommandResult<'a> = {
    Value: 'a
    Message: Message }

type CommandExecutionResult = Result<string, string>
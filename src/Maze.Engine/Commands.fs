namespace Maze.Engine

type Command =
    | Move of Direction
    | GetPosition of AsyncReplyChannel<Position>
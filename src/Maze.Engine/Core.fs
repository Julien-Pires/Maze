namespace Maze.Engine

type Agent<'a> = MailboxProcessor<'a>

type Position = {
    X: int
    Y: int }
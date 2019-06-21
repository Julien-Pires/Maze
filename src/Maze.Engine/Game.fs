namespace Maze.Engine

type Input = string -> WorldResult

type Output = string -> unit

type Game = {
    Input: Input }

module Game =
    let Start dungeon =
        let world = World.start dungeon
        { Input = world }

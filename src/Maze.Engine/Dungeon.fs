namespace Maze.Engine

type Dungeon = {
    Character: Character * Position
    Map: Map }

module Dungeon =
    let move direction dungeon =
        let (character, position) = dungeon.Character 
        let movement =
            match direction with
            | Forward -> { X = 0; Y = 1 }
            | Backward -> { X = 0; Y = -1 }
            | Left -> { X = -1; Y = 0 }
            | Right -> { X = 1; Y = 0 }
        let target =
            { X = position.X + movement.X 
              Y = position.Y + movement.Y }
        if dungeon.Map |> Map.canMove position target then
            let message =
                if dungeon.Map |> Map.hasReachedExit target then 
                    "You moved to a new room, you can exit the dungeon"
                else 
                    "You moved to a new room"
            { Value = { dungeon with Character = (character, target) }
              Message = message }
        else
            { Value = dungeon
              Message = "You cannot move further" }

    let canLeave dungeon =
        dungeon.Map |> Map.hasReachedExit (snd dungeon.Character)    
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
            Ok { Value = { dungeon with Character = (character, target) }
                 Message = "Moved succesfully" }
        else
            Error "Failed to move"
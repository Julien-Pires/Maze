namespace Maze.Engine

type Dungeon = {
    Character: Character * Position
    Map: Map }

module Dungeon =
    let move direction dungeon =
        let (character, position) = dungeon.Character 
        let movement =
            match direction with
            | Direction.Forward -> { X = 0; Y = 1 }
            | Direction.Backward -> { X = 0; Y = -1 }
            | Direction.Left -> { X = -1; Y = 0 }
            | Direction.Right -> { X = 1; Y = 0 }
        let target =
            { X = position.X + movement.X 
              Y = position.Y + movement.Y }
        if Map.canMove position target dungeon.Map then 
            Success { Value = { dungeon with Character = (character, target) }
                      Message = "Moved succesfully" }
        else
            Failure "Failed to move"
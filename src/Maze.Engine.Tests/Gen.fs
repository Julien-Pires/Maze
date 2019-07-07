namespace Maze.Engine.Tests

open FsCheck
open Maze.Engine

type Room = {
    Position : Position
    Type : string
}

type RoomsGen() =
    static member Rooms() : Arbitrary<Room list> =
        let genWidth = Gen.choose(2, 20)
        let genHeight = Gen.choose(2, 20)
        let genRoomType = Arb.generate<RoomType>
        let createRooms width height roomType =
            [
                for i in 0 .. width do
                    for j in 0 .. height do
                        yield { Position = { X = i; Y = j }; Type = roomType.ToString() }
            ]
        let genMap = createRooms <!> genWidth <*> genHeight <*> genRoomType
        genMap |> Arb.fromGen
namespace Maze.Tests

open Expecto
open Swensen.Unquote
open Newtonsoft.Json
open Maze
open Maze.FSharp

type MapTest = { Rooms : Maze.Tests.Room list }

module Data_Tests =
    let config = { FsCheckConfig.defaultConfig with arbitrary = [typeof<RoomsGen>] }
    
    [<Tests>]
    let loadMapTests =
        let badData = "123456789xvbnghtu"
        let noRoomsData = "{ \"Rooms\": [] }"
        
        testList "Data/loadMap"
            [
                yield
                    testCase "should return error when data is bad formatted" <|
                        fun _ ->
                            let loadData = async { return Ok badData }
                            let result = Data.loadMap loadData |> Async.RunSynchronously
                            
                            test <@ result |> function | Ok _ -> false
                                                       | Error _ -> true @>
                        
                yield
                    testCase "should return an empty map when data contains no rooms" <|
                        fun _ ->
                            let loadData = async { return Ok noRoomsData }
                            let result = Data.loadMap loadData |> Async.RunSynchronously
                            
                            test <@ result |> function | Ok x -> x = { Rooms = Map.empty<Position, Room> }
                                                       | Error _ -> false @>
                            
                yield
                    testPropertyWithConfig config "should return a valid map when data contains rooms" <|
                        fun rooms ->
                            let expected =
                                rooms
                                |> List.map (fun c -> (c.Position, { Type = unsafeFromString<RoomType> false c.Type }))
                                |> fun c -> { Map.Rooms = c |> Map.ofList }
                            let text = JsonConvert.SerializeObject({ Rooms = rooms })
                            let loadData = async { return Ok text }
                            let result = Data.loadMap loadData |> Async.RunSynchronously
                            
                            test <@ result |> function | Ok x -> x = expected
                                                       | Error _ -> false @>
            ]
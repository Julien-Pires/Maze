namespace Maze.Engine.Tests

open Expecto
open FSharp.Control
open Swensen.Unquote
open Maze.Engine

module Channel_Tests =
    [<Tests>]
    let channelTests =
        testList "Channel/Output" [
            yield
                testCase "should return an empty seq when no element has been passed to the channel" <|
                    fun _ ->
                        let sut = Channel.create()
                        
                        test <@ sut.Output |> AsyncSeq.toList |> List.isEmpty @>
        ]
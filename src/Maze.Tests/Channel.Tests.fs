namespace Maze.Tests

open Expecto
open FSharp.Control
open Swensen.Unquote
open Maze

module Channel_Tests =
    let config = FsCheckConfig.defaultConfig
    
    [<Tests>]
    let channelCreateTests =
        testList "Channel/create" [
            yield
                testPropertyWithConfig config "should create a channel that sends value from endpoint A to B" <|
                    fun (value : int) ->
                        let sut = Channel.create()
                        let result = async {
                            do sut.EndpointA.Post value
                            return! sut.EndpointB.Receive() } |> Async.RunSynchronously
                        
                        test <@ result = value @>
                        
            yield
                testPropertyWithConfig config "should create a channel that sends value from endpoint B to A" <|
                    fun (value : int) ->
                        let sut = Channel.create()
                        let result = async {
                            do sut.EndpointB.Post value
                            return! sut.EndpointA.Receive() } |> Async.RunSynchronously
                        
                        test <@ result = value @>
        ]
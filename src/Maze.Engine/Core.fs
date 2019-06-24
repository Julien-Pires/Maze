﻿namespace Maze.Engine

open Maze.FSharp

type Position = {
    X: int
    Y: int }

type Direction =
    | Forward
    | Backward
    | Left
    | Right
    static member FromString s = fromString<Direction> s
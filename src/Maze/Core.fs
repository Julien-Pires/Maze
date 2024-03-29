﻿namespace Maze

type Message = string

type Position =
    { X: int
      Y: int }

type Direction =
    | Forward
    | Backward
    | Left
    | Right
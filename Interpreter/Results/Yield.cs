﻿using Bloc.Values;

namespace Bloc.Results;

public sealed class Yield : IResult
{
    public Value Value { get; }

    public Yield(Value value) => Value = value;
}
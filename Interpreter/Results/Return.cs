﻿using Bloc.Values;

namespace Bloc.Results
{
    public class Return : Result
    {
        public Value Value { get; }

        public Return() => Value = Void.Value;

        public Return(Value value) => Value = value;
    }
}
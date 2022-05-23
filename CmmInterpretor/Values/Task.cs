﻿using CmmInterpretor.Results;

namespace CmmInterpretor.Values
{
    public class Task : Value
    {
        public override ValueType Type => ValueType.Task;

        public override Value Copy() => this;

        public override bool Equals(IValue other)
        {
            if (other.Value is not Task task)
                return false;
            
            // TODO

            return true;
        }

        public override T Implicit<T>()
        {
            if (typeof(T) == typeof(Bool))
                return (Bool.True as T)!;

            if (typeof(T) == typeof(String))
                return (new String(ToString()) as T)!;

            if (typeof(T) == typeof(Task))
                return (this as T)!;

            throw new Throw($"Cannot implicitly cast task as {typeof(T).Name.ToLower()}");
        }

        public override IValue Explicit(ValueType type)
        {
            return type switch
            {
                ValueType.Bool => Bool.True,
                ValueType.String => new String(ToString()),
                ValueType.Task => this,
                _ => throw new Throw($"Cannot cast task as {type.ToString().ToLower()}")
            };
        }

        public override string ToString(int _) => "[task]";
    }
}

﻿namespace Bloc.Values;

public abstract class Value : IValue
{
    Value IValue.Value => this;

    internal abstract new ValueType GetType();
    internal string GetTypeName() => GetType().ToString().ToLower();

    internal virtual void Destroy() { }
    internal virtual Value Copy(bool assign = false) => this;
    internal virtual Value GetOrCopy(bool assign = false) => this;

    public abstract override string ToString();
    public abstract override int GetHashCode();
    public abstract override bool Equals(object other);

    public static bool operator ==(Value a, Value b) => a.Equals(b);
    public static bool operator !=(Value a, Value b) => !a.Equals(b);
}
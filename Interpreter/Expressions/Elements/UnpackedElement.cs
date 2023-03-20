﻿using System.Collections.Generic;
using Bloc.Expressions.Members;
using Bloc.Expressions;
using Bloc.Memory;
using Bloc.Results;
using Bloc.Values;

internal sealed record UnpackedElement(IExpression Expression) : IElement
{
    public IEnumerable<Value> GetElements(Call call)
    {
        var value = Expression.Evaluate(call).Value.GetOrCopy();

        if (value is not Array array)
            throw new Throw("Only an array can be unpacked using the array unpack syntax");

        foreach (var val in array.Values)
            yield return val.Value;
    }
}
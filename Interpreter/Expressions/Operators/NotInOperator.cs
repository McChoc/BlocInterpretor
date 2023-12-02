﻿using System.Linq;
using Bloc.Memory;
using Bloc.Results;
using Bloc.Utils.Helpers;
using Bloc.Values.Behaviors;
using Bloc.Values.Core;
using Bloc.Values.Types;

namespace Bloc.Expressions.Operators;

internal sealed record NotInOperator : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    internal NotInOperator(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public IValue Evaluate(Call call)
    {
        var left = _left.Evaluate(call).Value;
        var right = _right.Evaluate(call).Value;

        right = ReferenceHelper.Resolve(right, call.Engine.Options.HopLimit).Value;

        if (left is INumeric numeric && right is Range range)
            return new Bool(!RangeHelper.Contains(range, numeric.GetDouble()));

        if (left is String sub && right is String str)
            return new Bool(!str.Value.Contains(sub.Value));

        if (right is Array array)
            return new Bool(!array.Values.Any(v => v.Value.Equals(left)));

        throw new Throw($"Cannot apply operator 'not in' on operands of types {left.GetTypeName()} and {right.GetTypeName()}");
    }
}
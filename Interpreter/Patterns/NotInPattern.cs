﻿using System.Linq;
using Bloc.Memory;
using Bloc.Utils.Helpers;
using Bloc.Values.Behaviors;
using Bloc.Values.Core;
using Bloc.Values.Types;

namespace Bloc.Patterns;

internal sealed record NotInPattern : IPatternNode
{
    private readonly Value _value;

    public NotInPattern(Value value)
    {
        _value = value;
    }

    public bool Matches(Value value, Call call)
    {
        var left = value;
        var right = ReferenceHelper.Resolve(_value, call.Engine.Options.HopLimit).Value;

        if (left is INumeric numeric && right is Range range)
            return !RangeHelper.Contains(range, numeric.GetDouble());

        if (left is String sub && right is String str)
            return !str.Value.Contains(sub.Value);

        if (right is Array array)
            return !array.Values.Any(v => v.Value.Equals(left));

        return false;
    }

    public bool HasAssignment()
    {
        return false;
    }
}
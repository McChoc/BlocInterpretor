﻿using Bloc.Memory;
using Bloc.Utils.Helpers;
using Bloc.Values;

namespace Bloc.Expressions.Operators;

internal sealed record BitwiseXorAssignment : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    internal BitwiseXorAssignment(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public IValue Evaluate(Call call)
    {
        var left = _left.Evaluate(call);
        var right = _right.Evaluate(call).Value;

        return AssignmentHelper.CompoundAssign(left, right, BitwiseXorOperator.Operation, call);
    }
}
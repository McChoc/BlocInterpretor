﻿using Bloc.Memory;
using Bloc.Utils.Helpers;
using Bloc.Values.Core;

namespace Bloc.Expressions.Operators;

internal sealed record DivisionAssignment : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    internal DivisionAssignment(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public IValue Evaluate(Call call)
    {
        var left = _left.Evaluate(call);
        var right = _right.Evaluate(call).Value;

        return AssignmentHelper.CompoundAssign(left, right, DivisionOperator.Operation, call);
    }
}
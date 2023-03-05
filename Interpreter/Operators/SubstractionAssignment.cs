﻿using Bloc.Expressions;
using Bloc.Memory;
using Bloc.Utils.Helpers;
using Bloc.Values;

namespace Bloc.Operators;

internal sealed record SubstractionAssignment : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    internal SubstractionAssignment(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public IValue Evaluate(Call call)
    {
        var left = _left.Evaluate(call);
        var right = _right.Evaluate(call).Value;

        return AssignmentHelper.CompoundAssign(left, right, SubstractionOperator.Operation, call);
    }
}
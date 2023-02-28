﻿using Bloc.Expressions;
using Bloc.Interfaces;
using Bloc.Memory;
using Bloc.Results;
using Bloc.Utils.Helpers;
using Bloc.Values;

namespace Bloc.Operators
{
    internal sealed record GreaterEqual : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        internal GreaterEqual(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public IValue Evaluate(Call call)
        {
            var left = _left.Evaluate(call).Value;
            var right = _right.Evaluate(call).Value;

            left = ReferenceUtil.Dereference(left, call.Engine.HopLimit).Value;
            right = ReferenceUtil.Dereference(right, call.Engine.HopLimit).Value;

            if (left is IScalar leftScalar && right is IScalar rightScalar)
                return new Bool(leftScalar.GetDouble() >= rightScalar.GetDouble());

            throw new Throw($"Cannot apply operator '>=' on operands of types {left.GetType().ToString().ToLower()} and {right.GetType().ToString().ToLower()}");
        }
    }
}
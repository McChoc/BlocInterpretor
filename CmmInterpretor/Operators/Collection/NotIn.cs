﻿using CmmInterpretor.Memory;
using CmmInterpretor.Expressions;
using CmmInterpretor.Results;
using CmmInterpretor.Values;
using System.Linq;

namespace CmmInterpretor.Operators.Collection
{
    public class NotIn : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public NotIn(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public IValue Evaluate(Call call)
        {
            var leftValue = _left.Evaluate(call);
            var rightValue = _right.Evaluate(call);

            if (rightValue.Is(out Array? array))
                return new Bool(!array!.Values.Any(v => v.Equals(leftValue!)));

            if (leftValue.Is(out String? sub) && rightValue.Is(out String? str))
                return new Bool(!str!.Value.Contains(sub!.Value));

            throw new Throw($"Cannot apply operator 'not in' on operands of types {leftValue.Type.ToString().ToLower()} and {rightValue.Type.ToString().ToLower()}");
        }
    }
}

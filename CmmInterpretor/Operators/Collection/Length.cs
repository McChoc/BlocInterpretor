﻿using CmmInterpretor.Memory;
using CmmInterpretor.Expressions;
using CmmInterpretor.Results;
using CmmInterpretor.Values;

namespace CmmInterpretor.Operators.Collection
{
    public class Length : IExpression
    {
        private readonly IExpression _operand;

        public Length(IExpression operand) => _operand = operand;

        public IValue Evaluate(Call call)
        {
            var value = _operand.Evaluate(call);

            if (value.Is(out Array? arr))
                return new Number(arr!.Values.Count);

            if (value.Is(out String? str))
                return new Number(str!.Value.Length);

            throw new Throw($"Cannot apply operator 'len' type {value!.Type.ToString().ToLower()}");
        }
    }
}

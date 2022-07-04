﻿using System;
using System.Collections.Generic;
using CmmInterpretor.Expressions;
using CmmInterpretor.Memory;
using CmmInterpretor.Results;
using CmmInterpretor.Utils;
using CmmInterpretor.Values;
using ValueType = CmmInterpretor.Values.ValueType;

namespace CmmInterpretor.Operators.Bitwise
{
    internal class BitwiseAnd : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        internal BitwiseAnd(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public IValue Evaluate(Call call)
        {
            var left = _left.Evaluate(call);
            var right = _right.Evaluate(call);

            return TupleUtil.RecursivelyCall(left, right, Operation);
        }

        internal static IValue Operation(IValue left, IValue right)
        {
            if (left.Is(out Number? leftNumber) && right.Is(out Number? rightNumber))
                return new Number(leftNumber!.ToInt() & rightNumber!.ToInt());

            if (left.Is(out TypeCollection? leftType) && right.Is(out TypeCollection? rightType))
            {
                var types = new HashSet<ValueType>();

                foreach (ValueType type in Enum.GetValues(typeof(ValueType)))
                    if (leftType!.Value.Contains(type) && rightType!.Value.Contains(type))
                        types.Add(type);

                return new TypeCollection(types);
            }

            throw new Throw($"Cannot apply operator '&' on operands of types {left.Type.ToString().ToLower()} and {right.Type.ToString().ToLower()}");
        }
    }
}
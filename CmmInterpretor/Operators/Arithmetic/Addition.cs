﻿using CmmInterpretor.Memory;
using CmmInterpretor.Expressions;
using CmmInterpretor.Results;
using CmmInterpretor.Values;
using System.Collections.Generic;
using CmmInterpretor.Utils;

namespace CmmInterpretor.Operators.Arithmetic
{
    public class Addition : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public Addition(IExpression left, IExpression right)
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
                return new Number(leftNumber!.Value + rightNumber!.Value);

            if (left.Is(out Array? leftArray) && right.Is(out Array? rightArray))
            {
                var list = new List<IValue>();
                list.AddRange(((Array)leftArray!.Copy()).Values);
                list.AddRange(((Array)rightArray!.Copy()).Values);
                return new Array(list);
            }

#pragma warning disable IDE0028

            if (left.Is(out Array? array))
            {
                var list = new List<IValue>();
                list.AddRange(((Array)array!.Copy()).Values);
                list.Add(right.Copy());
                return new Array(list);
            }

            if (right.Is(out array))
            {
                var list = new List<IValue>();
                list.Add(left.Copy());
                list.AddRange(((Array)array!.Copy()).Values);
                return new Array(list);
            }

#pragma warning restore IDE0028

            if (left.Is(out String? leftString) && right.Is(out String? rightString))
                return new String(leftString!.Value + rightString!.Value);

            throw new Throw($"Cannot apply operator '+' on operands of types {left.Type.ToString().ToLower()} and {right.Type.ToString().ToLower()}");
        }
    }
}

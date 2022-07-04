﻿using CmmInterpretor.Expressions;
using CmmInterpretor.Memory;
using CmmInterpretor.Operators.Arithmetic;
using CmmInterpretor.Utils;
using CmmInterpretor.Values;

namespace CmmInterpretor.Operators.Assignment
{
    internal class PowerAssignment : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        internal PowerAssignment(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public IValue Evaluate(Call call)
        {
            var left = _left.Evaluate(call);
            var right = _right.Evaluate(call);

            return TupleUtil.RecursivelyCompoundAssign(left, right, Power.Operation);
        }
    }
}
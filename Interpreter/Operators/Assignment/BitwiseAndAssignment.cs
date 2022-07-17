﻿using Bloc.Expressions;
using Bloc.Memory;
using Bloc.Pointers;
using Bloc.Utils;

namespace Bloc.Operators
{
    internal class BitwiseAndAssignment : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        internal BitwiseAndAssignment(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public IPointer Evaluate(Call call)
        {
            var left = _left.Evaluate(call);
            var right = _right.Evaluate(call);

            return AssignmentUtil.CompoundAssign(left, right, BitwiseAnd.Operation, call);
        }
    }
}
﻿using CmmInterpretor.Memory;
using CmmInterpretor.Expressions;
using CmmInterpretor.Values;
using CmmInterpretor.Variables;
using CmmInterpretor.Results;

namespace CmmInterpretor.Operators.Assignment
{
    internal class BooleanAndAssignment : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        internal BooleanAndAssignment(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public IValue Evaluate(Call call)
        {
            var value = _left.Evaluate(call);

            if (value is not Variable variable)
                throw new Throw("You cannot assign a value to a literal");

            if (!value.Is(out Bool? @bool))
                throw new Throw("Cannot implicitly convert to bool");

            if (!@bool!.Value)
                return value.Value;

            value = _right.Evaluate(call).Value;

            value.Assign();
            variable.Value.Destroy();
            return variable.Value = value.Value;
        }
    }
}

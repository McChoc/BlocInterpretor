﻿using Bloc.Expressions;
using Bloc.Memory;
using Bloc.Pointers;
using Bloc.Results;
using Bloc.Utils;
using Bloc.Values;

namespace Bloc.Operators
{
    internal class PreDecrement : IExpression
    {
        private readonly IExpression _operand;

        internal PreDecrement(IExpression operand)
        {
            _operand = operand;
        }

        public IPointer Evaluate(Call call)
        {
            var value = _operand.Evaluate(call);

            return TupleUtil.RecursivelyCall(value, Operation);
        }

        private static IPointer Operation(IPointer value)
        {
            if (value is not Pointer pointer)
                throw new Throw("The operand of an increment must be a variable");

            if (!pointer.Get().Is(out Number? number))
                throw new Throw($"Cannot apply operator '--' on type {pointer.Get().GetType().ToString().ToLower()}");

            return pointer.Set(new Number(number!.Value - 1));
        }
    }
}
﻿using CmmInterpretor.Data;
using CmmInterpretor.Results;
using CmmInterpretor.Tokens;
using System.Collections.Generic;

namespace CmmInterpretor.Statements
{
    public class ThrowStatement : Statement
    {
        private readonly List<Token> _tokens;

        public ThrowStatement(List<Token> tokens) => _tokens = tokens;

        public override IResult Execute(Call call)
        {
            if (_tokens.Count == 0)
                return new Return();

            var result = Evaluator.Evaluate(_tokens, call);

            if (result is IValue value)
                return new Throw(value.Value);

            return result;
        }
    }
}

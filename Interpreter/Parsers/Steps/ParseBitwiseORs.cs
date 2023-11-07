﻿using System.Collections.Generic;
using Bloc.Expressions;
using Bloc.Expressions.Operators;
using Bloc.Tokens;
using Bloc.Utils.Constants;
using Bloc.Utils.Exceptions;
using Bloc.Utils.Extensions;

namespace Bloc.Parsers.Steps;

internal sealed class ParseBitwiseORs : IParsingStep
{
    private readonly IParsingStep _nextStep;

    public ParseBitwiseORs(IParsingStep nextStep)
    {
        _nextStep = nextStep;
    }

    public IExpression Parse(List<Token> tokens)
    {
        for (int i = tokens.Count - 1; i >= 0; i--)
        {
            if (tokens[i] is SymbolToken(Symbol.BIT_OR) @operator)
            {
                if (i == 0)
                    throw new SyntaxError(@operator.Start, @operator.End, "Missing the left part of logical OR");

                if (i == tokens.Count - 1)
                    throw new SyntaxError(@operator.Start, @operator.End, "Missing the right part of logical OR");

                var left = Parse(tokens.GetRange(..i));
                var right = _nextStep.Parse(tokens.GetRange((i + 1)..));

                return new BitwiseOrOperator(left, right);
            }
        }

        return _nextStep.Parse(tokens);
    }
}
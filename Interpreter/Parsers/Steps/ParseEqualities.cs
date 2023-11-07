﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Bloc.Expressions;
using Bloc.Expressions.Operators;
using Bloc.Tokens;
using Bloc.Utils.Constants;
using Bloc.Utils.Exceptions;
using Bloc.Utils.Extensions;
using Bloc.Utils.Helpers;

namespace Bloc.Parsers.Steps;

internal sealed class ParseEqualities : IParsingStep
{
    private readonly IParsingStep _nextStep;

    public ParseEqualities(IParsingStep nextStep)
    {
        _nextStep = nextStep;
    }

    public IExpression Parse(List<Token> tokens)
    {
        for (int i = tokens.Count - 1; i >= 0; i--)
        {
            if (IsEquality(tokens[i], out var @operator) && OperatorHelper.IsBinary(tokens, i))
            {
                if (i > tokens.Count - 1)
                    throw new SyntaxError(@operator.Start, @operator.End, "Missing the right part of equality");

                var left = Parse(tokens.GetRange(..i));
                var right = _nextStep.Parse(tokens.GetRange((i + 1)..));

                return @operator.Text == Symbol.IS_EQUAL
                    ? new EqualOperator(left, right)
                    : new NotEqualOperator(left, right);
            }
        }

        return _nextStep.Parse(tokens);
    }

    private static bool IsEquality(Token token, [NotNullWhen(true)] out TextToken? @operator)
    {
        if (token is SymbolToken(Symbol.IS_EQUAL or Symbol.NOT_EQUAL_0 or Symbol.NOT_EQUAL_1 or Symbol.NOT_EQUAL_2))
        {
            @operator = (TextToken)token;
            return true;
        }
        else
        {
            @operator = null;
            return false;
        }
    }
}
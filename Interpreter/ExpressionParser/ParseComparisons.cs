﻿using System.Collections.Generic;
using Bloc.Constants;
using Bloc.Exceptions;
using Bloc.Expressions;
using Bloc.Operators;
using Bloc.Tokens;
using Bloc.Utils.Extensions;

namespace Bloc;

internal static partial class ExpressionParser
{
    private static IExpression ParseComparisons(List<Token> tokens, int precedence)
    {
        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            if (tokens[i] is SymbolToken(Symbol.COMPARISON) @operator)
            {
                if (i == 0)
                    throw new SyntaxError(@operator.Start, @operator.End, "Missing the left part of comparison");

                if (i > tokens.Count - 1)
                    throw new SyntaxError(@operator.Start, @operator.End, "Missing the right part of comparison");

                var left = ParseComparisons(tokens.GetRange(..i), precedence);
                var right = Parse(tokens.GetRange((i + 1)..), precedence - 1);

                return new ComparisonOperator(left, right);
            }
        }

        return Parse(tokens, precedence - 1);
    }
}
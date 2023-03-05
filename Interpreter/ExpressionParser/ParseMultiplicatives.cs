﻿using System;
using System.Collections.Generic;
using Bloc.Constants;
using Bloc.Exceptions;
using Bloc.Expressions;
using Bloc.Operators;
using Bloc.Tokens;
using Bloc.Utils.Extensions;

namespace Bloc;

internal static partial class ExpressionParser
{
    private static IExpression ParseMultiplicatives(List<Token> tokens, int precedence)
    {
        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            if (IsMultiplicative(tokens[i], out var @operator))
            {
                if (i == 0)
                    throw new SyntaxError(@operator!.Start, @operator.End, "Missing the left part of multiplicative");

                if (i > tokens.Count - 1)
                    throw new SyntaxError(@operator!.Start, @operator.End, "Missing the right part of multiplicative");

                var left = ParseMultiplicatives(tokens.GetRange(..i), precedence);
                var right = Parse(tokens.GetRange((i + 1)..), precedence - 1);

                return @operator!.Text switch
                {
                    Symbol.TIMES =>     new MultiplicationOperator(left, right),
                    Symbol.SLASH =>     new DivisionOperator(left, right),
                    Symbol.REMAINDER => new RemainderOperator(left, right),
                    Symbol.MODULO =>    new ModuloOperator(left, right),
                    _ => throw new Exception()
                };
            }
        }

        return Parse(tokens, precedence - 1);
    }

    private static bool IsMultiplicative(Token token, out TextToken? @operator)
    {
        if (token is SymbolToken(
            Symbol.TIMES or
            Symbol.SLASH or
            Symbol.REMAINDER or
            Symbol.MODULO))
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
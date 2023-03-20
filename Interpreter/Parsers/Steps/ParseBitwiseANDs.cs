﻿using System.Collections.Generic;
using Bloc.Expressions;
using Bloc.Expressions.Operators;
using Bloc.Tokens;
using Bloc.Utils.Constants;
using Bloc.Utils.Exceptions;
using Bloc.Utils.Extensions;

namespace Bloc.Parsers.Steps;

internal sealed class ParseBitwiseANDs : IParsingStep
{
    public IParsingStep? NextStep { get; init; }

    public ParseBitwiseANDs(IParsingStep? nextStep)
    {
        NextStep = nextStep;
    }

    public IExpression Parse(List<Token> tokens)
    {
        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            if (tokens[i] is SymbolToken(Symbol.BIT_AND) @operator)
            {
                if (i == 0)
                    throw new SyntaxError(@operator.Start, @operator.End, "Missing the left part of logical AND");

                if (i == tokens.Count - 1)
                    throw new SyntaxError(@operator.Start, @operator.End, "Missing the right part of logical AND");

                var left = Parse(tokens.GetRange(..i));
                var right = NextStep!.Parse(tokens.GetRange((i + 1)..));

                return new BitwiseAndOperator(left, right);
            }
        }

        return NextStep!.Parse(tokens);
    }
}
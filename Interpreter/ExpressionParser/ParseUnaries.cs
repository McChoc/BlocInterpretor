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
    private static IExpression ParseUnaries(List<Token> tokens, int precedence)
    {
        if (tokens.Count == 0)
            throw new SyntaxError(0, 0, "Missing value");

        if (IsPrefix(tokens[0], out var prefix))
        {
            var operand = ParseUnaries(tokens.GetRange(1..), precedence);

            return prefix!.Text switch
            {
                Symbol.PLUS         => new PositiveOperator(operand),
                Symbol.MINUS        => new NegativeOperator(operand),
                Symbol.BIT_NOT      => new ComplementOperator(operand),
                Symbol.BOOL_NOT     => new NegationOperator(operand),
                Symbol.INCREMENT    => new IncrementPrefix(operand),
                Symbol.DECREMENT    => new DecrementPrefix(operand),
                Symbol.BIT_INV      => new ComplementPrefix(operand),
                Symbol.BOOL_INV     => new NegationPrefix(operand),
                Keyword.LEN         => new LengthOperator(operand),
                Keyword.CHR         => new CharacterOperator(operand),
                Keyword.ORD         => new OrdinalOperator(operand),
                Keyword.REF         => new RefOperator(operand),
                Keyword.VAL         => new ValOperator(operand),
                Keyword.VAL_VAL     => new ValValOperator(operand),
                Keyword.LET         => new LetOperator(operand),
                Keyword.LET_NEW     => new LetNewOperator(operand),
                Keyword.NEW         => new NewOperator(operand),
                Keyword.CONST_NEW   => new ConstNewOperator(operand),
                Keyword.DELETE      => new DeleteOperator(operand),
                Keyword.GLOBAL      => new GlobalOperator(operand),
                Keyword.NONLOCAL    => new NonlocalOperator(operand),
                Keyword.PARAM       => new ParamOperator(operand),
                Keyword.AWAIT       => new AwaitOperator(operand),
                Keyword.NEXT        => new NextOperator(operand),
                Keyword.NAMEOF      => new NameofOperator(operand),
                Keyword.TYPEOF      => new TypeofOperator(operand),
                Keyword.EVAL        => new EvalOperator(operand),
                _ => throw new Exception()
            };
        }

        if (IsPostfix(tokens[^1], out var postfix))
        {
            var operand = ParseUnaries(tokens.GetRange(..^1), precedence);

            return postfix!.Text switch
            {
                Symbol.INCREMENT    => new IncrementPostfix(operand),
                Symbol.DECREMENT    => new DecrementPostfix(operand),
                Symbol.BIT_INV      => new ComplementPostfix(operand),
                Symbol.BOOL_INV     => new NegationPostfix(operand),
                Symbol.QUESTION     => new NullableOperator(operand),
                _ => throw new Exception()
            };
        }

        return Parse(tokens, precedence - 1);
    }

    private static bool IsPrefix(Token token, out TextToken? @operator)
    {
        if (token is SymbolToken(
            Symbol.PLUS or
            Symbol.MINUS or
            Symbol.BIT_NOT or
            Symbol.BOOL_NOT or
            Symbol.INCREMENT or
            Symbol.DECREMENT or
            Symbol.BIT_INV or
            Symbol.BOOL_INV))
        {
            @operator = (TextToken)token;
            return true;
        }

        if (token is KeywordToken(
            Keyword.LEN or
            Keyword.CHR or
            Keyword.ORD or
            Keyword.REF or
            Keyword.VAL or
            Keyword.VAL_VAL or
            Keyword.LET or
            Keyword.LET_NEW or
            Keyword.NEW or
            Keyword.CONST_NEW or
            Keyword.DELETE or
            Keyword.GLOBAL or
            Keyword.NONLOCAL or
            Keyword.PARAM or
            Keyword.AWAIT or
            Keyword.NEXT or
            Keyword.NAMEOF or
            Keyword.TYPEOF or
            Keyword.EVAL))
        {
            @operator = (TextToken)token;
            return true;
        }

        @operator = null;
        return false;
    }

    private static bool IsPostfix(Token token, out TextToken? @operator)
    {
        if (token is SymbolToken(
                Symbol.INCREMENT or
                Symbol.DECREMENT or
                Symbol.BIT_INV or
                Symbol.BOOL_INV or
                Symbol.QUESTION))
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
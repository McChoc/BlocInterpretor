﻿using CmmInterpretor.Utils.Exceptions;
using CmmInterpretor.Expressions;
using CmmInterpretor.Extensions;
using CmmInterpretor.Operators.Arithmetic;
using CmmInterpretor.Operators.Bitwise;
using CmmInterpretor.Operators.Boolean;
using CmmInterpretor.Operators.Character;
using CmmInterpretor.Operators.Collection;
using CmmInterpretor.Operators.Info;
using CmmInterpretor.Operators.Reference;
using CmmInterpretor.Operators.Type;
using CmmInterpretor.Tokens;
using System.Collections.Generic;

namespace CmmInterpretor
{
    public static partial class ExpressionParser
    {
        private static IExpression ParseUnaries(List<Token> tokens, int precedence)
        {
            if (tokens.Count == 0)
                throw new SyntaxError("Missing value");

            if (tokens[0] is { type: TokenType.Operator or TokenType.Keyword, value: "+" or "-" or "~" or "!" or "++" or "--" or "~~" or "!!" or "len" or "chr" or "ord" or "val" or "ref" or "new" or "nameof" or "typeof" })
            {
                var operand = ParseUnaries(tokens.GetRange(1..), precedence);

                return tokens[0].Text switch
                {
                    "+" => new Plus(operand),
                    "-" => new Minus(operand),
                    "~" => new Complement(operand),
                    "!" => new Negation(operand),
                    "++" => new PreIncrement(operand),
                    "--" => new PreDecrement(operand),
                    "~~" => new PreComplement(operand),
                    "!!" => new PreNegation(operand),
                    "len" => new Length(operand),
                    "chr" => new Character(operand),
                    "ord" => new Ordinal(operand),
                    "val" => new Value(operand),
                    "ref" => new Reference(operand),
                    "new" => new Allocation(operand),
                    "nameof" => new Nameof(operand),
                    "typeof" => new Typeof(operand),
                    _ => throw new System.Exception(),
                };
            }

            if (tokens[^1] is { type: TokenType.Operator, value: "++" or "--" or "~~" or "!!" or "?" })
            {
                var operand = ParseUnaries(tokens.GetRange(..^1), precedence);

                return tokens[^1].Text switch
                {
                    "++" => new PostIncrement(operand),
                    "--" => new PostDecrement(operand),
                    "~~" => new PostComplement(operand),
                    "!!" => new PostNegation(operand),
                    "?" => new Nullable(operand),
                    _ => throw new System.Exception(),
                };
            }

            return Parse(tokens, precedence - 1);
        }
    }
}

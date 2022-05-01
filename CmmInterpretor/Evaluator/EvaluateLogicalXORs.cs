﻿using CmmInterpretor.Data;
using CmmInterpretor.Exceptions;
using CmmInterpretor.Extensions;
using CmmInterpretor.Results;
using CmmInterpretor.Tokens;
using System.Collections.Generic;

namespace CmmInterpretor
{
    public static partial class Evaluator
    {
        private static IResult EvaluateLogicalXORs(List<Token> expr, Call call, int precedence)
        {
            for (int i = expr.Count - 1; i >= 0; i--)
            {
                if (expr[i] is { type: TokenType.Operator, value: "^" })
                {
                    if (i == 0)
                        throw new SyntaxError("Missing the left part of logical XOR");

                    if (i == expr.Count - 1)
                        throw new SyntaxError("Missing the right part of logical XOR");

                    var a = EvaluateLogicalXORs(expr.GetRange(..i), call, precedence);

                    if (a is not IValue aa)
                        return a;

                    var b = Evaluate(expr.GetRange((i + 1)..), call, precedence - 1);

                    if (b is not IValue bb)
                        return b;

                    return Operator.Xor(aa, bb);
                }
            }

            return Evaluate(expr, call, precedence - 1);
        }
    }
}

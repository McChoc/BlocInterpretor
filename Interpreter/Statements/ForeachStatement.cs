﻿using System;
using System.Collections.Generic;
using Bloc.Expressions;
using Bloc.Memory;
using Bloc.Results;
using Bloc.Utils.Helpers;
using Bloc.Values;

namespace Bloc.Statements;

internal sealed class ForeachStatement : Statement
{
    private readonly bool _checked;

    internal required IExpression Name { get; init; }
    internal required IExpression Expression { get; init; }
    internal required Statement Statement { get; init; }

    internal ForeachStatement(bool @checked)
    {
        _checked = @checked;
    }

    internal override IEnumerable<IResult> Execute(Call call)
    {
        if (!EvaluateExpression(Name, call, out var identifier, out var exception))
        {
            yield return exception!;
            yield break;
        }

        if (!EvaluateExpression(Expression, call, out var value, out exception))
        {
            yield return exception!;
            yield break;
        }

        if (!Iter.TryImplicitCast(value!, out var iter, call))
        {
            yield return new Throw("Cannot implicitly convert to iter");
            yield break;
        }

        using var enumerator = iter.Iterate().GetEnumerator();

        int loopCount = 0;

        while (true)
        {
            try
            {
                if (!enumerator.MoveNext())
                    break;
            }
            catch (Throw e)
            {
                exception = e;
            }

            if (exception is not null)
            {
                yield return exception;
                yield break;
            }

            if (++loopCount > call.Engine.LoopLimit && _checked)
            {
                yield return new Throw("The loop limit was reached.");
                yield break;
            }

            bool @break = false;

            using (call.MakeScope())
            {
                VariableHelper.Define(identifier!, enumerator.Current, call);

                foreach (var result in ExecuteStatement(Statement, call))
                {
                    bool @continue = false;

                    switch (result)
                    {
                        case Continue:
                            @continue = true;
                            break;

                        case Break:
                            @break = true;
                            break;

                        case Yield:
                            yield return result;
                            break;

                        default:
                            yield return result;
                            yield break;
                    }

                    if (@continue || @break)
                        break;
                }
            }

            if (@break)
                break;
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Label, _checked, Name, Expression, Statement);
    }

    public override bool Equals(object other)
    {
        return other is ForeachStatement statement &&
            Label == statement.Label &&
            _checked == statement._checked &&
            Name == statement.Name &&
            Expression.Equals(statement.Expression) &&
            Statement == statement.Statement;
    }
}
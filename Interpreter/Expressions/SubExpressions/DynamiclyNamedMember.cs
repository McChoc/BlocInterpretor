﻿using System.Collections.Generic;
using Bloc.Memory;
using Bloc.Results;
using Bloc.Utils.Helpers;
using Bloc.Values;

namespace Bloc.Expressions.SubExpressions;

internal sealed record DynamiclyNamedMember(IExpression NameExpression, IExpression ValueExpression) : IMember
{
    public IEnumerable<(string, Value)> GetMembers(Call call)
    {
        var nameValue = NameExpression.Evaluate(call);
        var nameString = String.ImplicitCast(nameValue);
        var name = nameString.Value;

        if (!IdentifierHelper.IsIdentifierValid(name))
            throw new Throw("Invalid identifier name");

        var value = ValueExpression.Evaluate(call).Value.GetOrCopy();

        if (value is Void)
            throw new Throw("'void' is not assignable");

        yield return (name, value);
    }
}

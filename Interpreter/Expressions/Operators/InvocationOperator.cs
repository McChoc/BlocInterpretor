﻿using System.Collections.Generic;
using Bloc.Funcs;
using Bloc.Identifiers;
using Bloc.Memory;
using Bloc.Results;
using Bloc.Utils.Attributes;
using Bloc.Utils.Helpers;
using Bloc.Values.Behaviors;
using Bloc.Values.Core;
using Bloc.Values.Types;

namespace Bloc.Expressions.Operators;

[Record]
internal sealed partial class InvocationOperator : IExpression
{
    private readonly IExpression _expression;
    private readonly List<Argument> _arguments;

    internal InvocationOperator(IExpression expression, List<Argument> arguments)
    {
        _expression = expression;
        _arguments = arguments;
    }

    public IValue Evaluate(Call call)
    {
        var value = _expression.Evaluate(call).Value;
        value = ReferenceHelper.Resolve(value, call.Engine.Options.HopLimit).Value;

        if (value is not IInvokable invokable)
            throw new Throw("The '()' operator can only be applied to funcs and types");

        var args = new List<Value?>();
        var kwargs = new Dictionary<string, Value>();

        foreach (var argument in _arguments)
        {
            var val = argument.Expression?.Evaluate(call).Value.GetOrCopy();

            if (val is Void)
                throw new Throw("'void' is not assignable");

            switch (argument.Type)
            {
                case ArgumentType.Positional:
                    args.Add(val);
                    break;

                case ArgumentType.Keyword:
                    var name = argument.Identifier!.GetName(call);

                    if (kwargs.ContainsKey(name))
                        throw new Throw($"Parameter named '{name}' cannot be specified multiple times");

                    kwargs.Add(name, val!);
                    break;

                case ArgumentType.UnpackedArray:
                    val = ReferenceHelper.Resolve(val!, call.Engine.Options.HopLimit).Value;

                    if (!Iter.TryImplicitCast(val, out var iter, call))
                        throw new Throw("Cannot implicitly convert to iter");

                    args.AddRange(IterHelper.CheckedIterate(iter, call.Engine.Options));
                    break;

                case ArgumentType.UnpackedStruct:
                    if (val is not Struct @struct)
                        throw new Throw("Only a struct can be unpacked using the struct unpack syntax");

                    foreach (var (key, variable) in @struct.Values)
                    {
                        if (kwargs.ContainsKey(key))
                            throw new Throw($"Parameter named '{key}' cannot be specified multiple times");

                        kwargs.Add(key, variable.Value);
                    }
                    break;
            }
        }

        return invokable.Invoke(args, kwargs, call);
    }

    internal sealed record Argument(INamedIdentifier? Identifier, IExpression? Expression, ArgumentType Type);
}
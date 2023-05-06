﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bloc.Core;
using Bloc.Pointers;
using Bloc.Results;
using Bloc.Values;
using Bloc.Variables;

namespace Bloc.Memory;

public sealed class Call
{
    private readonly int _stack;

    public Engine Engine { get; }

    internal VariableCollection Captures { get; }
    internal VariableCollection Params { get; }
    internal List<Scope> Scopes { get; }

    internal Call(Engine engine)
    {
        Engine = engine;
        Captures = new();
        Params = new();
        Scopes = new();
        MakeScope();
    }

    internal Call(Call parent, VariableCollection captures, VariableCollection @params)
        : this(parent.Engine)
    {
        _stack = parent._stack + 1;

        if (_stack > Engine.Options.StackLimit)
            throw new Throw("The stack limit was reached");

        Captures = captures;
        Params = @params;
    }

    internal Scope MakeScope()
    {
        return new Scope(this);
    }

    internal void Destroy()
    {
        while (Scopes.Count > 0)
            Scopes[^1].Dispose();
    }

    public UnresolvedPointer Get(string name)
    {
        Stack<StackVariable> stack;

        Variable? local = null;
        Variable? param = null;
        Variable? nonLocal = null;
        Variable? global = null;

        foreach (var scope in Scopes)
        {
            if (scope.Variables.TryGetValue(name, out stack))
            {
                local = stack.Peek();
                break;
            }
        }

        if (Params.Variables.TryGetValue(name, out stack))
            param = stack.Peek();

        if (Captures.Variables.TryGetValue(name, out stack))
            nonLocal = stack.Peek();

        if (Engine.GlobalScope.Variables.TryGetValue(name, out stack))
            global = stack.Peek();

        return new UnresolvedPointer(name, local, param, nonLocal, global);
    }

    public VariablePointer Set(bool mask, bool mutable, string name, Value value)
    {
        if (!mask && Scopes[^1].Variables.ContainsKey(name))
            throw new Throw($"Variable '{name}' was already defined in scope");

        var variable = new StackVariable(mutable, name, value, Scopes[^1]);

        Scopes[^1].Add(variable);

        return new VariablePointer(variable);
    }

    internal VariableCollection ValueCapture()
    {
        var captures = new VariableCollection();

        foreach (var scope in Scopes)
        {
            foreach (var (name, originalStack) in scope.Variables)
            {
                if (!captures.Variables.ContainsKey(name))
                    captures.Variables.Add(name, new());

                var stack = captures.Variables[name];

                foreach (var variable in originalStack.Reverse())
                    stack.Push(new(false, name, variable.Value.GetOrCopy(true), captures));
            }
        }

        return captures;
    }

    internal VariableCollection ReferenceCapture()
    {
        var captures = new VariableCollection();

        foreach (var scope in Scopes)
        {
            foreach (var (name, originalStack) in scope.Variables)
            {
                if (!captures.Variables.ContainsKey(name))
                    captures.Variables.Add(name, new());

                var stack = captures.Variables[name];

                foreach (var variable in originalStack.Reverse())
                    stack.Push(new(false, name, new Reference(new VariablePointer(variable)), captures));
            }
        }

        return captures;
    }
}
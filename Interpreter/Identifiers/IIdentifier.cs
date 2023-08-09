﻿using Bloc.Memory;
using Bloc.Values.Core;

namespace Bloc.Identifiers;

internal interface IIdentifier
{
    IValue Define(Value value, Call call, bool mask = false, bool mutable = true);
}
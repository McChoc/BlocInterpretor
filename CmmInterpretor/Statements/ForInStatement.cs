﻿using CmmInterpretor.Memory;
using CmmInterpretor.Expressions;
using CmmInterpretor.Results;
using CmmInterpretor.Variables;
using System.Collections.Generic;
using CmmInterpretor.Interfaces;

namespace CmmInterpretor.Statements
{
    public class ForInStatement : Statement
    {
        public string VariableName { get; set; } = default!;
        public IExpression Iterable { get; set; } = default!;
        public List<Statement> Statements { get; set; } = default!;

        public override Result? Execute(Call call)
        {
            try
            {
                var value = Iterable.Evaluate(call);

                if (value.Value is not IIterable iter)
                    return new Throw("You can only iterate over a range, a string or an array.");

                int loopCount = 0;
                var labels = GetLabels(Statements);

                foreach (var item in iter.Iterate())
                {
                    loopCount++;

                    if (loopCount > call.Engine.LoopLimit)
                        return new Throw("The loop limit was reached.");

                    try
                    {
                        call.Push();

                        call.Set(VariableName, new StackVariable(item, VariableName, call.Scopes[^1]));

                        var r = ExecuteBlockInLoop(Statements, labels, call);

                        if (r is Continue)
                            continue;
                        else if (r is Break)
                            break;
                        else if (r is not null)
                            return r;
                    }
                    finally
                    {
                        call.Pop();
                    }
                }
            }
            catch (Result result)
            {
                return result;
            }

            return null;
        }
    }
}

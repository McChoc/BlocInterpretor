﻿using System.Collections.Generic;
using Bloc.Expressions;
using Bloc.Parsers.Steps;
using Bloc.Tokens;

namespace Bloc.Parsers;

internal static class ExpressionParser
{
    private static IParsingStep FirstStep { get; }

    static ExpressionParser()
    {
        IParsingStep step;

        step = new ParseAtoms();
        step = new ParsePrimaries(step);
        step = new ParseUnaries(step);
        step = new ParseSelectives(step);
        step = new ParseFroms(step);
        step = new ParseQueries(step);
        step = new ParseExponentiations(step);
        step = new ParseMultiplicatives(step);
        step = new ParseAdditives(step);
        step = new ParseShifts(step);
        step = new ParseBitwiseANDs(step);
        step = new ParseBitwiseXORs(step);
        step = new ParseBitwiseORs(step);
        step = new ParseMatchAssignments(step);
        step = new ParseRanges(step);
        step = new ParseComparisons(step);
        step = new ParseRelations(step);
        step = new ParseEqualities(step);
        step = new ParseBooleanANDs(step);
        step = new ParseBooleanXORs(step);
        step = new ParseBooleanORs(step);
        step = new ParseCoalescings(step);
        step = new ParseCatches(step);
        step = new ParseConditionals(step);
        step = new ParseAssignments(step);
        step = new ParseFuncs(step);
        step = new ParseTuples(step);

        FirstStep = step;
    }

    internal static IExpression Parse(List<IToken> tokens)
    {
        return FirstStep.Parse(tokens);
    }
}
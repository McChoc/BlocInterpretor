﻿using System.Collections.Generic;
using System.Linq;
using Bloc.Tokens;
using Bloc.Utils.Constants;

namespace Bloc.Utils.Helpers;

internal static class CodeBlockHelper
{
    internal static bool IsCodeBlock(List<Token> tokens)
    {
        return tokens switch
        {
            [] => true,
            [BracesToken token, ..] when IsCodeBlock(token.Tokens) => true,
            [KeywordToken token, ..] when IsControlFlowKeyword(token) || IsLoopKeyword(token) => true,
            [WordToken(Keyword.UNCHECKED), KeywordToken token, ..] when IsLoopKeyword(token) => true,
            [IIdentifierToken, SymbolToken(Symbol.COLON), BracesToken token, ..] when IsCodeBlock(token.Tokens) => true,
            [IIdentifierToken, SymbolToken(Symbol.COLON), KeywordToken token, ..] when IsControlFlowKeyword(token) || IsLoopKeyword(token) => true,
            [IIdentifierToken, SymbolToken(Symbol.COLON), WordToken(Keyword.UNCHECKED), KeywordToken token, ..] when IsLoopKeyword(token) => true,
            _ when tokens.Any(x => x is SymbolToken(Symbol.SEMICOLON)) => true,
            _ => false,
        };

        static bool IsControlFlowKeyword(KeywordToken token)
        {
            return token.Text is
                Keyword.IF or
                Keyword.UNLESS or
                Keyword.SWITCH or
                Keyword.LOCK or
                Keyword.TRY;
        }

        static bool IsLoopKeyword(KeywordToken token)
        {
            return token.Text is
                Keyword.DO or
                Keyword.WHILE or
                Keyword.UNTIL or
                Keyword.LOOP or
                Keyword.REPEAT or
                Keyword.FOR or
                Keyword.FOREACH;
        }
    }
}
﻿using Bloc.Identifiers;

namespace Bloc.Tokens;

internal sealed class WordToken : TextToken, IKeywordToken, IStaticIdentifierToken
{
    internal WordToken(int start, int end, string text)
        : base(start, end, text) { }

    public INamedIdentifier GetIdentifier()
    {
        return new StaticIdentifier(Text);
    }
}
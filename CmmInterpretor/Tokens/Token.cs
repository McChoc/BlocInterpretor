﻿using System;

namespace CmmInterpretor.Tokens
{
    internal class Token
    {
        internal int Start { get; }
        internal int End { get; }
        internal virtual TokenType Type { get; }
        internal virtual string Text { get; }

        private protected Token(int start, int end)
        {
            Start = start;
            End = end;
            Text = "";
        }

        internal Token(int start, int end, TokenType type, string text)
        {
            if (type == TokenType.Literal)
                throw new Exception();

            Start = start;
            End = end;
            Type = type;
            Text = text;
        }

        internal void Deconstruct(out TokenType type, out string text)
        {
            type = Type;
            text = Text;
        }

        public override bool Equals(object other)
        {
            if (other is not Token token)
                return false;

            return Type == token.Type && Text == token.Text;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Type, Text);
        }
    }
}

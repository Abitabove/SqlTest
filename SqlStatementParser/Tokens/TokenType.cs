namespace SqlStatementParser.Tokens 
{
    public enum TokenType 
    {
        And,
        AsciiStringLiteral,
        Asterisk,
        Between,
        From,        
        NotDefined,
        CloseParenthesis,
        Comma,
        DateTimeValue,
        Dot,
        Equals,
        Greater,
        In,
        Invalid,
        Less,
        Like,
        NotEquals,
        NotIn,
        NotLike,
        Number,       
        Or,
        OpenParenthesis,
        Select,
        Semicolon,
        SequenceTerminator,
        StringValue,
        Table,
        Where,
        WhiteSpace
    }
}
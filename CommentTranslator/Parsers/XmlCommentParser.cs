﻿using System.Collections.Generic;

namespace CommentTranslator.Parsers
{
    public class XmlCommentParser : CommentParser
    {
        public XmlCommentParser()
        {
            Tags = new List<CommentTag>
            {
                //Multi line comment
                new CommentTag(){
                    Start = "<!--",
                    End = "-->",
                    Name = "multiline"
                }
            };
        }
    }
}

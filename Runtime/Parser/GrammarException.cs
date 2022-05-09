using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public class GrammarException : CaretException {
        public readonly IToken Token;

        public override Caret Caret {
            get { return Token == null ? Caret.InvalidCaret : Token.Caret; }
        }

        public override string Hint {
            get { return Token == null ? "" : Token.ToString(); }
        }

        public GrammarException(IToken token, string format, params object[] values)
                    : base(format, values) {
            Token = token;
        }

        public GrammarException(IToken token, Exception innerException,
                                string format, params object[] values)
                    : base(innerException, format, values) {
            Token = token;
        }
    }
}

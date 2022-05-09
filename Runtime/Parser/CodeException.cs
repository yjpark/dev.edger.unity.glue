using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public class CodeException : CaretException {
        private readonly Caret _Caret;

        public override Caret Caret {
            get { return _Caret; }
        }

        public CodeException(Caret caret, string format, params object[] values)
                    : base(format, values) {
            _Caret = caret;
        }

        public CodeException(Caret caret, Exception innerException,
                                string format, params object[] values)
                    : base(innerException, format, values) {
            _Caret = caret;
        }
    }
}

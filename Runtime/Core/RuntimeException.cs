using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public class RuntimeException : CaretException {
        public readonly IRunnable GlueSource = null;
        private readonly Caret _Caret = Caret.InvalidCaret;
        public readonly string GlueStackTrace = null;

        public RuntimeException(IRunnable glueSource,
                                string format, params object[] values)
                    : base(format, values) {
            GlueSource = glueSource;
        }

        public RuntimeException(IRunnable glueSource, Caret caret, string glueStackTrace,
                             string format, params object[] values)
                    : base(format, values) {
            GlueSource = glueSource;
            _Caret = caret;
            GlueStackTrace = glueStackTrace;
        }

        public RuntimeException(IRunnable glueSource, Caret caret, string glueStackTrace,
                             Exception innerException,
                             string format, params object[] values)
                    : base(innerException, format, values) {
            GlueSource = glueSource;
            _Caret = caret;
            GlueStackTrace = glueStackTrace;
        }

        public override Caret Caret {
            get { return _Caret; }
        }
    }
}

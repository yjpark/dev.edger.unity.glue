using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public class RuntimeBreak : RuntimeException {
        public RuntimeBreak(IRunnable source,
                                string format, params object[] values)
                    : base(source, format, values, values) {
        }

        public RuntimeBreak(IRunnable source, Caret caret, string stackTrace,
                             string format, params object[] values)
                    : base(source, caret, stackTrace, format, values) {
        }

        public RuntimeBreak(IRunnable source, Caret caret, string stackTrace,
                             Exception innerException,
                             string format, params object[] values)
                    : base(source, caret, stackTrace, innerException, format, values) {
        }
    }
}

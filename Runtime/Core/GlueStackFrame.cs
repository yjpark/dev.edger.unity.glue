using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public sealed class GlueStackFrame {
        public readonly Caret Caret;
        public readonly string Info;

        public GlueStackFrame(Caret caret, string info) {
            Caret = caret;
            Info = info;
        }
    }
}

using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.None_ {
    public class Attach : Pipe<None, bool, string> {
        public Attach(string param1) : base(param1) {
        }

        protected override bool Do(Runtime runtime, Caret caret, None input, string scriptPath) {
            return runtime.Attach(scriptPath);
        }
    }
}

using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Runtime_ {
    public class Attach : Pipe<Runtime, bool, string> {
        public Attach(string param1) : base(param1) {
        }

        protected override bool Do(Runtime runtime, Caret caret, Runtime input, string scriptPath) {
            return input.Attach(scriptPath);
        }
    }
}

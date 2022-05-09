using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.None_ {
    public class Call : Pipe<None, bool, string> {
        public Call(string param1) : base(param1) {
        }

        protected override bool Do(Runtime runtime, Caret caret, None input, string procName) {
            return runtime.Execute(procName);
        }
    }
}

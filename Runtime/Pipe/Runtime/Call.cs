using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Runtime_ {
    public class Call : Pipe<Runtime, bool, string> {
        public Call(string param1) : base(param1) {
        }

        protected override bool Do(Runtime runtime, Caret caret, Runtime input, string procName) {
            return input.Execute(procName);
        }
    }
}

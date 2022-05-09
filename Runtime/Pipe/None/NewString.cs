using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.None_ {
    public class NewString : Pipe<None, string, string> {
        public NewString(string param1) : base(param1) {
        }

        protected override string Do(Runtime runtime, Caret caret, None input, string val) {
            return val;
        }
    }
}

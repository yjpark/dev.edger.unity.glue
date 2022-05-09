using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Object_ {
    public class Format : Pipe<object, string, string> {
        public Format(string param1) : base(param1) {
        }

        protected override string Do(Runtime runtime, Caret caret, object input, string format) {
            return string.Format(format, input);
        }
    }
}

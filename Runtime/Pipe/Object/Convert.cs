using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Object_ {
    public class Convert : Pipe<object, string> {
        protected override string Do(Runtime runtime, Caret caret, object input) {
            return Convertor.Convert(input, false);
        }
    }
}

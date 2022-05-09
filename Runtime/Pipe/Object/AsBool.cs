using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Object_ {
    public class AsBool : Pipe<object, bool> {
        protected override bool Do(Runtime runtime, Caret caret, object input) {
            return (bool)input;
        }
    }
}

using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Object_ {
    public class AsInt : Pipe<object, int> {
        protected override int Do(Runtime runtime, Caret caret, object input) {
            return (int)input;
        }
    }
}

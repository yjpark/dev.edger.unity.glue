using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Object_ {
    public class AsLong : Pipe<object, long> {
        protected override long Do(Runtime runtime, Caret caret, object input) {
            return (long)input;
        }
    }
}

using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Object_ {
    public class AsFloat : Pipe<object, float> {
        protected override float Do(Runtime runtime, Caret caret, object input) {
            return (float)input;
        }
    }
}

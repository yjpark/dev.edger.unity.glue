using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Object_ {
    public class AsDouble : Pipe<object, double> {
        protected override double Do(Runtime runtime, Caret caret, object input) {
            return (double)input;
        }
    }
}

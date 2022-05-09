using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Object_ {
    public class Save : StatePipe<object, object> {
        public Save(string stateKey) : base(stateKey) {
        }

        protected override object Do(Runtime runtime, Caret caret, object input) {
            runtime.Save(StateKey, input);
            return input;
        }
    }
}

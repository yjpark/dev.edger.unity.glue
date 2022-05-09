using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.None_ {
    public class Delete : StatePipe<None, object> {
        public Delete(string stateKey) : base(stateKey) {
        }

        protected override object Do(Runtime runtime, Caret caret, None input) {
            return runtime.Delete(StateKey);
        }
    }
}

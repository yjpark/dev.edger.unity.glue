using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.None_ {
    public class Take : StatePipe<None, object> {
        public Take(string stateKey) : base(stateKey) {
        }

        protected override object Do(Runtime runtime, Caret caret, None input) {
            return runtime.Take(StateKey);
        }
    }
}

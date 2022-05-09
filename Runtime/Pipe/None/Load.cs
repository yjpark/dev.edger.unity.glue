using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.None_ {
    public class Load : StatePipe<None, object> {
        public Load(string stateKey) : base(stateKey) {
        }

        protected override object Do(Runtime runtime, Caret caret, None input) {
            return runtime.Load(StateKey);
        }
    }
}

using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe {
    public abstract class Load<T> : StatePipe<None, T> {
        public Load(string stateKey) : base(stateKey) {
        }

        protected override T Do(Runtime runtime, Caret caret, None input) {
            return runtime.Load<T>(StateKey);
        }
    }
}

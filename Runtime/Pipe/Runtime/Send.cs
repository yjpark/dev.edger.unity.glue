using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Runtime_ {
    public class Send : StatePipe<Runtime, Runtime, string> {
        public Send(string stateKey, string param1) : base(stateKey, param1) {
            CheckStateKey(param1);
        }

        protected override Runtime Do(Runtime runtime, Caret caret, Runtime input, string remoteStateKey) {
            object val = runtime.Load(StateKey);
            if (val != null) {
                input.Save(remoteStateKey, val);
            }
            return input;
        }
    }
}

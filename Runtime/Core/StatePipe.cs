using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public abstract class StatePipe<TI, TO> : Pipe<TI, TO> {
        public readonly string StateKey;

        protected StatePipe(string stateKey) : base() {
            StateKey = stateKey;
        }

        public override string GetParams() {
            return StateKey;
        }
    }
}

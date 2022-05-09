using System;

using Edger.Unity.Context;
using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Env_ {
    public class GetBus : Pipe<Env, Bus> {
        public GetBus() : base() {
        }

        protected override Bus Do(Runtime runtime, Caret caret, Env input) {
            return input.GetAspect<Bus>();
        }
    }
}

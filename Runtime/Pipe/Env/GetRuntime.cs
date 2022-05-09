using System;

using Edger.Unity.Context;
using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Env_ {
    public class GetRuntime : Pipe<Env, Runtime> {
        public GetRuntime() : base() {
        }

        protected override Runtime Do(Runtime runtime, Caret caret, Env input) {
            return input.GetAspect<Runtime>();
        }
    }
}

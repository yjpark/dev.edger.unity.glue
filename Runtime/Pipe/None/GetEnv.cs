using System;

using Edger.Unity.Context;
using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.None_ {
    public class GetEnv : Pipe<None, Env> {
        public GetEnv() : base() {
        }

        protected override Env Do(Runtime runtime, Caret caret, None input) {
            return runtime.Env;
        }
    }
}

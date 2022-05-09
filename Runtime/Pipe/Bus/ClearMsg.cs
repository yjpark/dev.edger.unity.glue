using System;

using Edger.Unity.Context;
using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Bus_ {

    public class ClearMsg : Pipe<Bus, bool, string> {
        public ClearMsg(string param1) : base(param1) {
        }

        protected override bool Do(Runtime runtime, Caret caret, Bus input,
                                        string msg) {
            return input.Clear(msg, runtime);
        }
    }
}

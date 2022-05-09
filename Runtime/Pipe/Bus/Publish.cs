using System;

using Edger.Unity.Context;
using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Bus_ {
    public class Publish : Pipe<Bus, bool, string> {
        public Publish(string param1) : base(param1) {
        }

        protected override bool Do(Runtime runtime, Caret caret, Bus input,
                                        string msg) {
            return input.Publish(msg, runtime);
        }
    }
}

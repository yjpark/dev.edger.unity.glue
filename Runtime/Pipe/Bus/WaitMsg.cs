using System;

using Edger.Unity.Context;
using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Bus_ {
    public class WaitMsg : Pipe<Bus, bool, string, string> {
        public WaitMsg(string param1, string param2) : base(param1, param2) {
        }

        protected override bool Do(Runtime runtime, Caret caret, Bus input,
                                        string msg, string procName) {
            return input.WaitMsg(msg, (Bus bus, string _msg, bool isNew) => {
                runtime.Execute(procName);
            });
        }
    }
}

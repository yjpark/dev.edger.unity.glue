using System;

using Edger.Unity.Context;
using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Bus_ {
    public class AddSub : Pipe<Bus, Bus, string, string> {
        public AddSub(string param1, string param2) : base(param1, param2) {
        }

        protected override Bus Do(Runtime runtime, Caret caret, Bus input,
                                        string msg, string procName) {
            input.AddSub(msg, runtime, (Bus bus, string _msg) => {
                runtime.Execute(procName);
            });
            return input;
        }
    }
}

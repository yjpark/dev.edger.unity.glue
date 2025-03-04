﻿using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.Object_ {
    public class Info : Pipe<object, object, string> {
        public Info(string param1) : base(param1) {
        }

        protected override object Do(Runtime runtime, Caret caret, object input, string msg) {
            msg = string.Format("{0}\n\tinput: {1}", msg,
                    input == null ? "null" : string.Format("<{0}> [{1}]", input.GetType().Name, input));
            runtime._LogStackTraceByTrace(false, msg);
            return input;
        }
    }
}

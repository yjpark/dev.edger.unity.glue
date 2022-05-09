using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe {
    public abstract class Cast<T> : Pipe<None, T, T> {
        public Cast(string param1) : base(param1) {
        }

        protected override T Do(Runtime runtime, Caret caret, None input, T val) {
            return val;
        }
    }
}

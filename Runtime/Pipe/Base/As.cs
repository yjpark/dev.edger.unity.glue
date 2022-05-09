using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe {
    public abstract class As<T> : Pipe<object, T> where T : class {
        protected override T Do(Runtime runtime, Caret caret, object input) {
            T result = input as T;
            if (result == null) {
                throw NewRuntimeException(runtime, caret, "Type Mismatched: <{0}> -> {1}", typeof(T).Name, input);
            }
            return result;
        }
    }
}

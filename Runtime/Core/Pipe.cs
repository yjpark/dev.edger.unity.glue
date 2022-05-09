using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public interface IPipe : IRunnable {
        Type InputType { get; }
        Type OutputType { get; }

        bool IsValidInputType(Type type);
        bool IsValidInput(object obj);
        bool IsValidOutputType(Type type);
        bool IsValidOutput(object obj);

        string GetParams();

        /*
         * The reason to pass runtime and caret around is to reuse the
         * pipes, any pipes with same code will be cached.
         */
        object Process(Runtime runtime, Caret caret, object input);
    }

    public abstract class BasePipe<TI, TO> : Runnable, IPipe {
        private readonly Type _InputType = typeof(TI);
        public Type InputType {
            get { return _InputType; }
        }

        private readonly Type _OutputType = typeof(TO);
        public Type OutputType {
            get { return _OutputType; }
        }

        public bool IsValidInputType(Type type) {
            return type != null && _InputType.IsAssignableFrom(type);
        }

        public bool IsValidInput(object input) {
            return input != null && _InputType.IsAssignableFrom(input.GetType());
        }

        public bool IsValidOutputType(Type type) {
            return type != null && _OutputType.IsAssignableFrom(type);
        }

        public bool IsValidOutput(object output) {
            return output != null && _OutputType.IsAssignableFrom(output.GetType());
        }

        public override string ToString() {
            return string.Format("|>{0}", Code);
        }

        protected override string GetCode() {
            return string.Format("{0}<{1}, {2}>({3})", GetType().Name,
                                  InputType.Name, OutputType.Name, GetParams());
        }

        public virtual string GetParams() {
            return string.Empty;
        }

        public object Process(Runtime runtime, Caret caret, object input) {
            TO output;
            output = Do(runtime, caret, (TI)input);
            if (runtime.DebugMode) {
                Debug(runtime, caret, "{0} -> {1}", input, output);
            }
            if (output == null) {
                return None.Instance;
            }
            return output;
        }

        /* input will never be null */
        protected abstract TO Do(Runtime runtime, Caret caret, TI input);
    }

    public abstract class Pipe<TI, TO> : BasePipe<TI, TO> {
        protected void CheckStateKey(string key) {
            if (!ParamConsts.IsStateKey(key)) {
                throw new ArgumentException(string.Format(
                            "Invalid State Key: {0} -> {1}", Code, key));
            }
        }

        protected void InitParam<TP>(string _param, out string param, out bool isState, out TP val) {
            param = _param;
            isState = ParamConsts.IsStateKey(param);
            if (isState) {
                val = default(TP);
            } else {
                val = Param.ParseValue<TP>(param);
            }
        }
    }
}

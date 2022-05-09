using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public static class ParamConsts {
        public const string PrefixStateKey = "$";

        public static bool IsStateKey(string key) {
            return key.StartsWith(PrefixStateKey);
        }
    }

    public static class Param {
        public static T ParseValue<T>(string param) {
            return Convertor.Parse<T>(param);
        }

        public static T GetStateValue<T>(Runtime runtime, Caret caret, IPipe pipe, string param) {
            T state = runtime.Load<T>(param);
            if (runtime.DebugMode) {
                pipe.Debug(runtime, caret, "State Param: {0} -> {1}", param, state);
            }
            return state;
        }
    }
}

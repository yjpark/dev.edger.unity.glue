using System;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    /*
    * It's a bit tricky to handle null in the Pipe system, so
    * disallow null, a None object will be used to wrap null.
    */
    public sealed class None {
        public static None Instance = new None();

        public static bool IsNone(object obj) {
            return obj == Instance;
        }

        private None() {
        }

        public override string ToString() {
            return "None";
        }
    }
}


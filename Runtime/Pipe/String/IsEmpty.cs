using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe {
    public class IsEmpty : Pipe<string, bool> {
        protected override bool Do(Runtime runtime, Caret caret, string input) {
            return string.Empty == input;
        }
    }
}

using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe {
    //Note: the Input type here have to be object, since Pipe.Process will convert any null value to None.
    public class IsNullOrEmpty : Pipe<object, bool> {
        protected override bool Do(Runtime runtime, Caret caret, object input) {
            if (None.IsNone(input)) {
                return true;
            }
            //Note: if compare without the cast, will always return false.
            string str = input as string;
            if (str != null) {
                return string.Empty == str;
            }
            return false;
        }
    }
}

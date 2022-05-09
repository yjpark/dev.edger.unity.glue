using System;

using Edger.Unity.Glue;

namespace Edger.Unity.Glue.Pipe.None_ {
    public class RandomInt : Pipe<None, int, int, int> {
        public RandomInt(string param1, string param2) : base(param1, param2) {
        }

        /*
         * min: The inclusive lower bound of the random number returned.
         * max: The exclusive upper bound of the random number returned. maxValue must be greater than or equal to min.
         */
        protected override int Do(Runtime runtime, Caret caret, None input, int min, int max) {
            System.Random random = new System.Random();
            return random.Next(min, max);
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public abstract class StatePipe<TI, TO, TP1> : StatePipe<TI, TO> {
        //SILP:PARAM_DECLARE(1);
        public readonly string Param1;                                //__SILP__
        public readonly bool IsState1;                                //__SILP__
        public readonly TP1 Value1;                                   //__SILP__

        protected StatePipe(string stateKey, string param1) : base(stateKey) {
            //SILP:PARAM_INIT(1);
            InitParam(param1, out Param1, out IsState1, out Value1);  //__SILP__
        }

        public override string GetParams() {
            return string.Format("{0}, {1}", StateKey, Param1);
        }

        protected sealed override TO Do(Runtime runtime, Caret caret, TI input) {
            //SILP:PARAM_VALUE(1);
            TP1 value1 = Value1;                                                  //__SILP__
            if (IsState1) {                                                       //__SILP__
                value1 = Param.GetStateValue<TP1>(runtime, caret, this, Param1);  //__SILP__
            }                                                                     //__SILP__
            return Do(runtime, caret, input, value1);
        }

        protected abstract TO Do(Runtime runtime, Caret caret, TI input, TP1 value1);
    }

    public abstract class StatePipe<TI, TO, TP1, TP2> : StatePipe<TI, TO> {
        //SILP:PARAM_DECLARE(1);
        public readonly string Param1;                                //__SILP__
        public readonly bool IsState1;                                //__SILP__
        public readonly TP1 Value1;                                   //__SILP__
        //SILP:PARAM_DECLARE(2);
        public readonly string Param2;                                //__SILP__
        public readonly bool IsState2;                                //__SILP__
        public readonly TP2 Value2;                                   //__SILP__

        protected StatePipe(string stateKey, string param1, string param2) : base(stateKey) {
            //SILP:PARAM_INIT(1);
            InitParam(param1, out Param1, out IsState1, out Value1);  //__SILP__
            //SILP:PARAM_INIT(2);
            InitParam(param2, out Param2, out IsState2, out Value2);  //__SILP__
        }

        public override string GetParams() {
            return string.Format("{0}, {1}, {2}", StateKey, Param1, Param2);
        }

        protected sealed override TO Do(Runtime runtime, Caret caret, TI input) {
            //SILP:PARAM_VALUE(1);
            TP1 value1 = Value1;                                                  //__SILP__
            if (IsState1) {                                                       //__SILP__
                value1 = Param.GetStateValue<TP1>(runtime, caret, this, Param1);  //__SILP__
            }                                                                     //__SILP__
            //SILP:PARAM_VALUE(2);
            TP2 value2 = Value2;                                                  //__SILP__
            if (IsState2) {                                                       //__SILP__
                value2 = Param.GetStateValue<TP2>(runtime, caret, this, Param2);  //__SILP__
            }                                                                     //__SILP__
            return Do(runtime, caret, input, value1, value2);
        }

        protected abstract TO Do(Runtime runtime, Caret caret, TI input, TP1 value1, TP2 value2);
    }

    public abstract class StatePipe<TI, TO, TP1, TP2, TP3> : StatePipe<TI, TO> {
        //SILP:PARAM_DECLARE(1);
        public readonly string Param1;                                //__SILP__
        public readonly bool IsState1;                                //__SILP__
        public readonly TP1 Value1;                                   //__SILP__
        //SILP:PARAM_DECLARE(2);
        public readonly string Param2;                                //__SILP__
        public readonly bool IsState2;                                //__SILP__
        public readonly TP2 Value2;                                   //__SILP__
        //SILP:PARAM_DECLARE(3);
        public readonly string Param3;                                //__SILP__
        public readonly bool IsState3;                                //__SILP__
        public readonly TP3 Value3;                                   //__SILP__

        protected StatePipe(string stateKey, string param1, string param2, string param3) : base(stateKey) {
            //SILP:PARAM_INIT(1);
            InitParam(param1, out Param1, out IsState1, out Value1);  //__SILP__
            //SILP:PARAM_INIT(2);
            InitParam(param2, out Param2, out IsState2, out Value2);  //__SILP__
            //SILP:PARAM_INIT(3);
            InitParam(param3, out Param3, out IsState3, out Value3);  //__SILP__
        }

        public override string GetParams() {
            return string.Format("{0}, {1}, {2}, {3}", StateKey, Param1, Param2, Param3);
        }

        protected sealed override TO Do(Runtime runtime, Caret caret, TI input) {
            //SILP:PARAM_VALUE(1);
            TP1 value1 = Value1;                                                  //__SILP__
            if (IsState1) {                                                       //__SILP__
                value1 = Param.GetStateValue<TP1>(runtime, caret, this, Param1);  //__SILP__
            }                                                                     //__SILP__
            //SILP:PARAM_VALUE(2);
            TP2 value2 = Value2;                                                  //__SILP__
            if (IsState2) {                                                       //__SILP__
                value2 = Param.GetStateValue<TP2>(runtime, caret, this, Param2);  //__SILP__
            }                                                                     //__SILP__
            //SILP:PARAM_VALUE(3);
            TP3 value3 = Value3;                                                  //__SILP__
            if (IsState3) {                                                       //__SILP__
                value3 = Param.GetStateValue<TP3>(runtime, caret, this, Param3);  //__SILP__
            }                                                                     //__SILP__
            return Do(runtime, caret, input, value1, value2, value3);
        }

        protected abstract TO Do(Runtime runtime, Caret caret, TI input, TP1 value1, TP2 value2, TP3 value3);
    }
}

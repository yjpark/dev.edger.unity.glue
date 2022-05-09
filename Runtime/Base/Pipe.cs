using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public abstract class Pipe<TI, TO, TP1> : Pipe<TI, TO> {
        //SILP:PARAM_DECLARE(1);
        public readonly string Param1;                                //__SILP__
        public readonly bool IsState1;                                //__SILP__
        public readonly TP1 Value1;                                   //__SILP__

        protected Pipe(string param1) {
            //SILP:PARAM_INIT(1);
            InitParam(param1, out Param1, out IsState1, out Value1);  //__SILP__
        }

        public override string GetParams() {
            return Param1;
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

    public abstract class Pipe<TI, TO, TP1, TP2> : Pipe<TI, TO> {
        //SILP:PARAM_DECLARE(1);
        public readonly string Param1;                                //__SILP__
        public readonly bool IsState1;                                //__SILP__
        public readonly TP1 Value1;                                   //__SILP__
        //SILP:PARAM_DECLARE(2);
        public readonly string Param2;                                //__SILP__
        public readonly bool IsState2;                                //__SILP__
        public readonly TP2 Value2;                                   //__SILP__

        protected Pipe(string param1, string param2) {
            //SILP:PARAM_INIT(1);
            InitParam(param1, out Param1, out IsState1, out Value1);  //__SILP__
            //SILP:PARAM_INIT(2);
            InitParam(param2, out Param2, out IsState2, out Value2);  //__SILP__
        }

        public override string GetParams() {
            return string.Format("{0}, {1}", Param1, Param2);
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

        protected abstract TO Do(Runtime runtime, Caret caret, TI input,
                                    TP1 value1, TP2 value2);
    }

    public abstract class Pipe<TI, TO, TP1, TP2, TP3> : Pipe<TI, TO> {
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

        protected Pipe(string param1, string param2, string param3) {
            //SILP:PARAM_INIT(1);
            InitParam(param1, out Param1, out IsState1, out Value1);  //__SILP__
            //SILP:PARAM_INIT(2);
            InitParam(param2, out Param2, out IsState2, out Value2);  //__SILP__
            //SILP:PARAM_INIT(3);
            InitParam(param3, out Param3, out IsState3, out Value3);  //__SILP__
        }

        public override string GetParams() {
            return string.Format("{0}, {1}, {2}", Param1, Param2, Param3);
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

        protected abstract TO Do(Runtime runtime, Caret caret, TI input,
                                    TP1 value1, TP2 value2, TP3 value3);
    }

    public abstract class Pipe<TI, TO, TP1, TP2, TP3, TP4> : Pipe<TI, TO> {
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
        //SILP:PARAM_DECLARE(4);
        public readonly string Param4;                                //__SILP__
        public readonly bool IsState4;                                //__SILP__
        public readonly TP4 Value4;                                   //__SILP__

        protected Pipe(string param1, string param2, string param3, string param4) {
            //SILP:PARAM_INIT(1);
            InitParam(param1, out Param1, out IsState1, out Value1);  //__SILP__
            //SILP:PARAM_INIT(2);
            InitParam(param2, out Param2, out IsState2, out Value2);  //__SILP__
            //SILP:PARAM_INIT(3);
            InitParam(param3, out Param3, out IsState3, out Value3);  //__SILP__
            //SILP:PARAM_INIT(4);
            InitParam(param4, out Param4, out IsState4, out Value4);  //__SILP__
        }

        public override string GetParams() {
            return string.Format("{0}, {1}, {2}, {3}", Param1, Param2, Param3, Param4);
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
            //SILP:PARAM_VALUE(4);
            TP4 value4 = Value4;                                                  //__SILP__
            if (IsState4) {                                                       //__SILP__
                value4 = Param.GetStateValue<TP4>(runtime, caret, this, Param4);  //__SILP__
            }                                                                     //__SILP__
            return Do(runtime, caret, input, value1, value2, value3, value4);
        }

        protected abstract TO Do(Runtime runtime, Caret caret, TI input,
                                    TP1 value1, TP2 value2, TP3 value3, TP4 value4);
    }

    public abstract class Pipe<TI, TO, TP1, TP2, TP3, TP4, TP5> : Pipe<TI, TO> {
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
        //SILP:PARAM_DECLARE(4);
        public readonly string Param4;                                //__SILP__
        public readonly bool IsState4;                                //__SILP__
        public readonly TP4 Value4;                                   //__SILP__
        //SILP:PARAM_DECLARE(5);
        public readonly string Param5;                                //__SILP__
        public readonly bool IsState5;                                //__SILP__
        public readonly TP5 Value5;                                   //__SILP__

        protected Pipe(string param1, string param2, string param3, string param4, string param5) {
            //SILP:PARAM_INIT(1);
            InitParam(param1, out Param1, out IsState1, out Value1);  //__SILP__
            //SILP:PARAM_INIT(2);
            InitParam(param2, out Param2, out IsState2, out Value2);  //__SILP__
            //SILP:PARAM_INIT(3);
            InitParam(param3, out Param3, out IsState3, out Value3);  //__SILP__
            //SILP:PARAM_INIT(4);
            InitParam(param4, out Param4, out IsState4, out Value4);  //__SILP__
            //SILP:PARAM_INIT(5);
            InitParam(param5, out Param5, out IsState5, out Value5);  //__SILP__
        }

        public override string GetParams() {
            return string.Format("{0}, {1}, {2}, {3}, {4}", Param1, Param2, Param3, Param4, Param5);
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
            //SILP:PARAM_VALUE(4);
            TP4 value4 = Value4;                                                  //__SILP__
            if (IsState4) {                                                       //__SILP__
                value4 = Param.GetStateValue<TP4>(runtime, caret, this, Param4);  //__SILP__
            }                                                                     //__SILP__
            //SILP:PARAM_VALUE(5);
            TP5 value5 = Value5;                                                  //__SILP__
            if (IsState5) {                                                       //__SILP__
                value5 = Param.GetStateValue<TP5>(runtime, caret, this, Param5);  //__SILP__
            }                                                                     //__SILP__
            return Do(runtime, caret, input, value1, value2, value3, value4, value5);
        }

        protected abstract TO Do(Runtime runtime, Caret caret, TI input,
                                    TP1 value1, TP2 value2, TP3 value3, TP4 value4, TP5 value5);
    }
}

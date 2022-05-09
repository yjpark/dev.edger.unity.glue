using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public abstract class Pipeline<TI, TO> : BasePipe<TI, TO> {
        private List<IPipe> _Pipes = new List<IPipe>();
        /*
         * Maintain the pipe -> caret mapping here, since the pipes might be reused
         * in Parser, so same instance will be used at multiple places.
         *
         * Pipeline will never been reused, and all pipe process are triggered by it.
         */
        private List<GlueStackFrame> _PipeFrames = new List<GlueStackFrame>();

        public Caret Caret {
            get {
                if (_PipeFrames.Count > 0) {
                    return _PipeFrames[0].Caret;
                }
                return Caret.InvalidCaret;
            }
        }

        public bool IsNop {
            get { return _Pipes.Count == 0; }
        }

        public void AddPipe(IPipe pipe, Caret caret) {
            Type objType = InputType;
            if (_Pipes.Count > 0) {
                objType = _Pipes[_Pipes.Count - 1].OutputType;
            }
            if (!pipe.IsValidInputType(objType)) {
                throw new CodeException(caret, "Invalid Pipe: {0} Type Mismatched: {1} -> {2}",
                                pipe, objType.FullName, pipe.InputType.FullName);
            }
            _Pipes.Add(pipe);
            _PipeFrames.Add(new GlueStackFrame(caret, pipe.Code));
        }

        public Type GetNextInputType() {
            if (_Pipes.Count == 0) {
                return InputType;
            } else {
                return _Pipes[_Pipes.Count - 1].OutputType;
            }
        }

        protected sealed override TO Do(Runtime runtime, Caret caret, TI input) {
            object obj = input;
            for (int i = 0; i < _Pipes.Count; i++) {
                IPipe pipe = _Pipes[i];
                GlueStackFrame pipeFrame = _PipeFrames[i];
                if (!pipe.IsValidInput(obj)) {
                    throw NewRuntimeException(runtime, pipeFrame.Caret, "Pipe Broken: {0} Type Mismatched: {1} -> {2}",
                                    pipe, pipe.InputType.FullName,
                                    obj == null ? "null" : obj.GetType().FullName);
                } else {
                    runtime._PushStackFrame(pipeFrame);
                    obj = pipe.Process(runtime, pipeFrame.Caret, obj);
                    runtime._PopStackFrame();
                }
            }
            if (!IsValidOutput(obj)) {
                throw NewRuntimeException(runtime, caret, "Pipeline Broken: Type Mismatched: {1} -> {2}",
                            OutputType.FullName, obj == null ? "null" : obj.GetType().FullName);
            }
            return (TO)obj;
        }

        protected override string GetCode() {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _Pipes.Count; i++) {
                if (i > 0) {
                    builder.Append(" |> ");
                }
                builder.Append(_Pipes[i].Code);
            }
            builder.Append(";");
            return builder.ToString();
        }
    }

    public class Pipeline : Pipeline<None, object> {
    }
}

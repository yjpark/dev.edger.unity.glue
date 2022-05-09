using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public interface IRunnable {
        string Code { get; }

        void Error(Runtime runtime, Caret caret, string format, params object[] values);
        void Info(Runtime runtime, Caret caret, string format, params object[] values);
        void Debug(Runtime runtime, Caret caret, string format, params object[] values);
        void Custom(Runtime runtime, Caret caret, string kind, string format, params object[] values);
    }

    public abstract class Runnable : IRunnable {
        public override string ToString() {
            return string.Format("[{0}: {1}]", GetType().FullName, Code);
        }

        private string _Code = null;
        public string Code {
            get {
                string result = _Code;
                if (result == null) {
                    _Code = GetCode();
                }
                return result;
            }
        }

        protected abstract string GetCode();

        private string GetFormat(Runtime runtime, Caret caret, string format) {
            StringBuilder builder = new StringBuilder();
            builder.Append(runtime.LogPrefix);
            builder.Append(caret.Source);
            builder.Append("<");
            builder.Append(caret.Line);
            builder.Append(": ");
            builder.Append(caret.Column);
            builder.Append("> ");
            builder.Append(format);
            if (runtime.LogDebug) {
                builder.Append("\n");
                builder.Append(runtime.FormatGlueStackTrace());
            }
            return builder.ToString();
        }

        protected RuntimeException NewRuntimeException(Runtime runtime, Caret caret, string format, params object[] values) {
            return new RuntimeException(this, caret, runtime.FormatGlueStackTrace(), format, values);
        }

        protected RuntimeBreak NewRuntimeBreak(Runtime runtime, Caret caret, string format, params object[] values) {
            return new RuntimeBreak(this, caret, runtime.FormatGlueStackTrace(), format, values);
        }

        public void Error(Runtime runtime, Caret caret, string format, params object[] values) {
            Log.AddLogWithStackTrace(runtime, LoggerConsts.ERROR, GetFormat(runtime, caret, format), values);
        }

        public void Info(Runtime runtime, Caret caret, string format, params object[] values) {
            if (runtime.DebugMode) {
                Log.AddLogWithStackTrace(runtime, LoggerConsts.INFO, GetFormat(runtime, caret, format), values);
            } else {
                Log.AddLog(this, LoggerConsts.INFO, GetFormat(runtime, caret, format), values);
            }
        }

        public void Debug(Runtime runtime, Caret caret, string format, params object[] values) {
            if (runtime.DebugMode) {
                Log.AddLogWithStackTrace(this, LoggerConsts.DEBUG, GetFormat(runtime, caret, format), values);
            } else {
                if (runtime.LogDebug) {
                    Log.AddLog(this, LoggerConsts.DEBUG, GetFormat(runtime, caret, format), values);
                }
            }
        }

        public void Custom(Runtime runtime, Caret caret, string kind, string format, params object[] values) {
            if (runtime.DebugMode) {
                Log.AddLogWithStackTrace(this, kind, GetFormat(runtime, caret, format), values);
            } else {
                Log.AddLog(this, kind, GetFormat(runtime, caret, format), values);
            }
        }
    }
}

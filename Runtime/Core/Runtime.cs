using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Glue {
    public static class RuntimeConsts {
        public const string GlueStakFramePrefix = "\t\t";

        //Temp Procedures Can Be Override.
        public const string PrefixTempProcedureName = "_";

        public static bool IsTempProcedureName(string name) {
            return name.StartsWith(PrefixTempProcedureName);
        }
    }

    [DisallowMultipleComponent()]
    public class Runtime : Env {
        private Dictionary<string, object> _States = new Dictionary<string, object>();
        private Dictionary<string, IScript> _Scripts = new Dictionary<string, IScript>();
        private Dictionary<string, Procedure> _Procedures = new Dictionary<string, Procedure>();

        private Stack<GlueStackFrame> _Stack = new Stack<GlueStackFrame>();
        private GlueStackFrame _LastTracedFrame = null;

        private RuntimeException _LastError = null;
        public RuntimeException LastError {
            get { return _LastError; }
        }

        private StringBuilder _StackBuilder = new StringBuilder(1024);

        public void GlueError(string format, params object[] values) {
            LogStackTrace(true, Log.GetMsg(format, values));
        }

        public void GlueInfo(string format, params object[] values) {
            LogStackTrace(false, Log.GetMsg(format, values));
        }

        public void GlueLog(bool isError, string format, params object[] values) {
            if (isError) {
                LogStackTrace(true, Log.GetMsg(format, values));
            } else {
                LogStackTrace(false, Log.GetMsg(format, values));
            }
        }

        private bool TryRun(string errMsg, string errParam, Action action) {
            //IProfiler profiler = Log.BeginSample(string.Format("Runtime.TryRun: {0} {1}", errMsg, errParam));
            bool result = true;
            int stackCount = _Stack.Count;
            try {
                action();
            } catch (RuntimeBreak e) {
                if (LogDebug) {
                    string msg = string.Format("{0} Break: {1} -> {2}",
                            errMsg, errParam, e.MessageWithCaret);
                    Debug(GetStackTrace(msg, false));
                }
            } catch (RuntimeException e) {
                _LastError = e;

                string msg = string.Format("{0} Failed: {1} -> {2}",
                        errMsg, errParam, e.MessageWithCaret);
                LogStackTrace(true, msg);
                result = false;
            }
            if (_Stack.Count < stackCount) {
                Error("Invalid Glue Stack: {0} -> {1}", stackCount, _Stack.Count);
            }
            while (_Stack.Count > stackCount) {
                _PopStackFrame();
            }
            //if (profiler != null) profiler.EndSample();
            return result;
        }

        public bool Has(string key) {
            return _States.ContainsKey(key);
        }

        public bool Delete(string key) {
            return _States.Remove(key);
        }

        public void Save(string key, object state) {
            _States[key] = state;
        }

        public object Load(string key) {
            object state = null;
            if (_States.TryGetValue(key, out state)) {
                return state;
            } else {
                GlueError("State Not Found: {0}", key);
            }
            return null;
        }

        public object Take(string key) {
            object state = null;
            if (_States.TryGetValue(key, out state)) {
                _States.Remove(key);
                return state;
            } else {
                GlueError("State Not Found: {0}", key);
            }
            return null;
        }

        private T Convert<T>(string key, object state) {
            if (state != null) {
                if (state is T) {
                    return (T)state;
                } else {
                    GlueError("State Type Mismatched: {0}: {1} -> {2}",
                            key, state.GetType().FullName, typeof(T).FullName);
                }
            } else {
                GlueError("State Not Found: <{0}>: {1}", typeof(T).FullName, key);
            }
            return default(T);
        }

        public T Load<T>(string key) {
            object state = Load(key);
            return Convert<T>(key, state);
        }

        public T Take<T>(string key) {
            object state = Take(key);
            return Convert<T>(key, state);
        }

        //Only supposed to be called from Script.cs
        internal bool AddProcedure(Procedure proc) {
            if (proc == null) {
                GlueError("Invalid Procedure: {0}", proc);
                return false;
            }
            if (!RuntimeConsts.IsTempProcedureName(proc.Name)) {
                Procedure old;
                if (_Procedures.TryGetValue(proc.Name, out old)) {
                    GlueError("Procedure Already Exist: {0} -> {1}",
                            old.Code, proc.Code);
                    return false;
                }
            }
            _Procedures[proc.Name] = proc;
            return true;
        }

        public bool HasProcedure(string name) {
            return _Procedures.ContainsKey(name);
        }

        public bool Execute(string name) {
            Procedure proc;
            if (_Procedures.TryGetValue(name, out proc)) {
                return TryRun("Execute", name, () => {
                        proc.Execute(this);
                });
            } else {
                GlueError("Procedure Not Exist: {0}", name);
                return false;
            }
        }

        public bool Attach(IScript script) {
            _Scripts[script.Path] = script;
            return TryRun("Attach", script.Path, () => {
                    script.AttachTo(this);
            });
        }

        public bool Attach(string path) {
            IScript script = Parser.GetScript(path);
            if (script == null) {
                GlueError("Script Not Found: {0}", path);
                return false;
            }
            return Attach(script);
        }

        private void AppendFrame(string prefix, GlueStackFrame frame) {
            Caret caret = frame.Caret;
            _StackBuilder.Append(prefix);
            _StackBuilder.Append(caret.Source);
            _StackBuilder.Append("<");
            _StackBuilder.Append(caret.Line);
            _StackBuilder.Append(": ");
            _StackBuilder.Append(caret.Column);
            _StackBuilder.Append(">\t");
            _StackBuilder.Append(frame.Info);
            _StackBuilder.Append("\n");
        }

        private string FormatGlueStackTrace(string prefix, bool byTrace) {
            _StackBuilder.Length = 0;
            bool skipFrame = false;
            if (byTrace && _LastTracedFrame != null) {
                AppendFrame(prefix, _LastTracedFrame);
                skipFrame = true;
            }
            foreach (GlueStackFrame frame in _Stack) {
                if (skipFrame) {
                    skipFrame = false;
                } else {
                    AppendFrame(prefix, frame);
                }
            }
            return _StackBuilder.ToString();
        }

        public string FormatGlueStackTrace() {
            return FormatGlueStackTrace(RuntimeConsts.GlueStakFramePrefix, false);
        }

        private string GetStackTrace(string msg, bool byTrace) {
            return string.Format("{0}\n\n{1}", msg,
                        FormatGlueStackTrace(RuntimeConsts.GlueStakFramePrefix, byTrace));
        }

        private void LogStackTrace(bool isError, string msg, bool byTrace) {
            if (isError) {
                Error(GetStackTrace(msg, byTrace));
            } else {
                Info(GetStackTrace(msg, byTrace));
            }
        }

        public void LogStackTrace(bool isError, string msg) {
            LogStackTrace(isError, msg, false);
        }

        public void _LogStackTraceByTrace(bool isError, string msg) {
            LogStackTrace(isError, msg, true);
        }

        /* Only Called by Procedure and Pipeline */
        internal void _PushStackFrame(GlueStackFrame frame, bool asTraceFrame=false) {
            _Stack.Push(frame);
            if (asTraceFrame) {
                _LastTracedFrame = frame;
            }
        }

        internal void _PopStackFrame() {
            if (_Stack.Count > 0) {
                _LastTracedFrame = _Stack.Pop();
            } else {
                Error("Invalid Glue Stack");
            }
        }
    }
}

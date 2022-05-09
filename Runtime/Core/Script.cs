using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public interface IScript : IRunnable {
        bool IsValid { get; }
        string Path { get; }
        void AttachTo(Runtime runtime);
        void ForEachProcedure(Action<Procedure> callback);
    }

    public class Script : Runnable, IScript {
        public bool IsValid {
            get { return true; }
        }

        private readonly string _Path;
        public string Path {
            get { return _Path; }
        }

        public readonly Procedure Init;

        private HashSet<string> _ProcedureNames = null;
        private List<Procedure> _Procedures = null;

        public Script(string path, Caret caret) {
            _Path = path;
            Init = new Procedure(caret, "");
        }

        public void AddProcedure(Procedure proc) {
            if (_Procedures == null) {
                _ProcedureNames = new HashSet<string>();
                _Procedures = new List<Procedure>();
            }
            if (_ProcedureNames.Contains(proc.Name)) {
                throw new CodeException(proc.Caret, "Name Conflicted: {0} -> {1}\n{2}",
                                Path, proc.Name, proc.Code);
            }
            _ProcedureNames.Add(proc.Name);
            _Procedures.Add(proc);
        }

        protected override string GetCode() {
            StringBuilder builder = new StringBuilder();
            builder.Append("'''\n");
            builder.Append(Init.Code);

            if (_Procedures != null) {
                foreach (var proc in _Procedures) {
                    builder.Append(proc.Code);
                }
            }
            builder.Append("\n'''");
            return builder.ToString();
        }

        public void ForEachProcedure(Action<Procedure> callback) {
            if (_Procedures != null) {
                foreach (var proc in _Procedures) {
                    callback(proc);
                }
            }
        }

        public void AttachTo(Runtime runtime) {
            List<Procedure> conflictedProcs = null;
            ForEachProcedure((Procedure proc) => {
                if (!RuntimeConsts.IsTempProcedureName(proc.Name)) {
                    if (runtime.HasProcedure(proc.Name)) {
                        Error(runtime, proc.Caret, "Script Procedure Conflict: {0} -> {1}{2}", Path, proc.Caret, proc.Name);
                        if (conflictedProcs == null) {
                            conflictedProcs = new List<Procedure>();
                        }
                        conflictedProcs.Add(proc);
                    }
                }
            });
            if (conflictedProcs != null) {
                StringBuilder builder = new StringBuilder();
                foreach (var proc in conflictedProcs) {
                    builder.Append("\t");
                    builder.Append(proc.Name);
                    builder.Append(" ");
                    builder.Append(SeparatorTypeConsts.ProcedureDeclaration);
                    builder.Append(" ");
                    builder.Append(proc.Code);
                    builder.Append("\n");
                }
                throw NewRuntimeException(runtime, Init.Caret, "Conflicted Procedures: {0}\n{1}",
                        Path, builder.ToString());
            }

            ForEachProcedure((Procedure proc) => {
                if (!runtime.AddProcedure(proc)) {
                    throw NewRuntimeException(runtime, proc.Caret,
                            "Attach Precedure Failed: {0} -> {1}{2}", Path, proc.Caret, proc.Name);
                }
            });
            Info(runtime, Init.Caret, "Script Attached: {0}", Path);
            if (!Init.IsNop) {
                Info(runtime, Init.Caret, "Initing Script: {0}", Path);
                Init.Execute(runtime);
            }
        }
    }
}

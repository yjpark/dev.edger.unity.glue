using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public class Procedure : Runnable {
        public readonly Caret Caret;
        public readonly string Name;

        private readonly GlueStackFrame _Frame;

        private int _ExecuteCount = 0;
        private int _SucceedCount = 0;

        public Procedure(Caret caret, string name) {
            Caret = caret;
            Name = name == null ? "" : name;

            _Frame = new GlueStackFrame(Caret, string.Format("[{0}]", Name));
        }

        public bool IsInit {
            get { return string.IsNullOrEmpty(Name); }
        }

        private Pipeline _SinglePipeline = null;

        private List<Pipeline> _MultiPipelines = null;

        public bool IsNop {
            get { return _SinglePipeline == null && _MultiPipelines == null; }
        }

        public void AddPipeline(Pipeline pipeline) {
            if (pipeline == null || pipeline.IsNop) return;

            if (_MultiPipelines != null) {
                _MultiPipelines.Add(pipeline);
            } else if (_SinglePipeline != null) {
                _MultiPipelines = new List<Pipeline>();
                _MultiPipelines.Add(_SinglePipeline);
                _SinglePipeline = null;
                _MultiPipelines.Add(pipeline);
            } else {
                _SinglePipeline = pipeline;
            }
        }

        protected override string GetCode() {
            StringBuilder builder = new StringBuilder();
            string prefix = "";
            if (!IsInit) {
                prefix = "    ";
                builder.Append(Name);
                builder.Append(" =>\n");
            }
            if (_SinglePipeline != null) {
                builder.Append(prefix);
                builder.Append(_SinglePipeline.Code);
            } else if (_MultiPipelines != null) {
                for (int i = 0; i < _MultiPipelines.Count; i++) {
                    builder.Append(prefix);
                    builder.Append(_MultiPipelines[i].Code);
                }
            } else {
                builder.Append("NOP");
            }
            return builder.ToString();
        }

        private void ExecutePipeline(Runtime runtime, Caret caret, int index, Pipeline pipeline) {
            object result = pipeline.Process(runtime, pipeline.Caret, None.Instance);
            if (runtime.LogDebug) {
                string indexStr = index < 0 ? "" : string.Format("[{0}]", index);
                Debug(runtime, caret, "{0}{1} -> {2}", Name, indexStr, result);
            }
        }

        public void Execute(Runtime runtime) {
            _ExecuteCount++;
            runtime._PushStackFrame(_Frame, true);
            if (_SinglePipeline != null) {
                ExecutePipeline(runtime, Caret, -1, _SinglePipeline);
            } else if (_MultiPipelines != null) {
                for (int i = 0; i < _MultiPipelines.Count; i++) {
                    ExecutePipeline(runtime, Caret, i, _MultiPipelines[i]);
                }
            } else {
                //Don't treat NOP case as error here
                if (runtime.LogDebug) {
                    Debug(runtime, Caret, "{0} -> NOP", Name);
                }
            }
            runtime._PopStackFrame();
            _SucceedCount++;
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public sealed class InvalidScript : Runnable, IScript {
        public bool IsValid {
            get { return false; }
        }

        private readonly string _Path;
        public string Path {
            get { return _Path; }
        }

        public readonly string Content;
        public readonly Exception Exception;

        public InvalidScript(string path, string content, Exception e) {
            _Path = path;
            Content = content;
            Exception = e;
        }

        protected override string GetCode() {
            StringBuilder builder = new StringBuilder();
            builder.Append(CaretException.GetMessage(Path, Exception));
            builder.Append("'''\n");
            builder.Append(Content);
            builder.Append("\n'''");
            if (Exception != null) {
                builder.Append(Exception.StackTrace);
            }
            return builder.ToString();
        }

        public void AttachTo(Runtime runtime) {
        }

        public void ForEachProcedure(Action<Procedure> callback) {
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public class PipeException : Exception {
        public readonly IPipe Pipe;

        public PipeException(IPipe pipe, string format, params object[] values)
                    : base(string.Format(format, values)) {
            Pipe = pipe;
        }

        public PipeException(IPipe pipe, Exception innerException,
                                string format, params object[] values)
                    : base(string.Format(format, values), innerException) {
            Pipe = pipe;
        }
    }
}

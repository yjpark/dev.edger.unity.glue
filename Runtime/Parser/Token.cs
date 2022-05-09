using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public interface IToken {
        Caret Caret { get; }
    }

    public enum SeparatorType {
        PipeSeparator,              // |>
        PipelineSeparator,          // ;
        ProcedureDeclaration,       // =>
    }

    public static class SeparatorTypeConsts {
        public const string PipeSeparator = "|>";
        public const string PipelineSeparator = ";";
        public const string ProcedureDeclaration = "=>";

        public static bool IsSeparator(string val) {
            return val == PipeSeparator
                    || val == PipelineSeparator
                    || val == ProcedureDeclaration;
        }
    }

    public class SeparatorToken : IToken {
        public static SeparatorToken ToSeparatorToken(Word word) {
            string val = word.Value;
            if (val == SeparatorTypeConsts.PipeSeparator) {
                return new SeparatorToken(word.Caret, SeparatorType.PipeSeparator);
            } else if (val == SeparatorTypeConsts.PipelineSeparator) {
                return new SeparatorToken(word.Caret, SeparatorType.PipelineSeparator);
            } else if (val == SeparatorTypeConsts.ProcedureDeclaration) {
                return new SeparatorToken(word.Caret, SeparatorType.ProcedureDeclaration);
            }
            return null;
        }

        private readonly Caret _Caret;
        public Caret Caret {
            get { return _Caret; }
        }

        public readonly SeparatorType Type;

        public SeparatorToken(Caret caret, SeparatorType type) {
            _Caret = caret;
            Type = type;
        }

        public override string ToString() {
            return string.Format("[{0}: {1}]", GetType().Name, Type);
        }
    }

    public class IdentityToken : IToken{
        private readonly Caret _Caret;
        public Caret Caret {
            get { return _Caret; }
        }

        public readonly string Name;

        public IdentityToken(Caret caret, string name) {
            _Caret = caret;
            Name = name;
        }

        public override string ToString() {
            return string.Format("[{0}: {1}]", GetType().Name, Name);
        }
    }

    public class PipeToken : IToken {
        private readonly Caret _Caret;
        public Caret Caret {
            get { return _Caret; }
        }

        private readonly static string[] EmptyParams = new string[]{};

        public readonly string Name;
        private List<string> _Params = null;

        public int ParamCount {
            get { return _Params == null ? 0 : _Params.Count; }
        }
        public string[] Params {
            get { return _Params == null ? EmptyParams : _Params.ToArray(); }
        }

        public PipeToken(Caret caret, string name) {
            _Caret = caret;
            Name = name;
        }

        public bool AddParam(string param) {
            if (_Params == null) {
                _Params = new List<string>();
            }
            _Params.Add(param);
            return true;
        }

        public string GetCode() {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name);
            builder.Append("(");
            if (_Params != null) {
                for (int i = 0; i < _Params.Count; i++) {
                    builder.Append(_Params[i]);
                    if (i < _Params.Count - 1) {
                        builder.Append(", ");
                    }
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        public override string ToString() {
            return string.Format("[{0}: {1}]", GetType().Name, GetCode());
        }
    }
}

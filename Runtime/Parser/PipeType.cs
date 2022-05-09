using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public class PipeType {
        public readonly Type Type;

        public readonly Type InputType;
        public readonly Type OutputType;

        public readonly int ParamCount;

        private readonly ConstructorInfo Constructor;

        public string Name {
            get { return Type.Name; }
        }

        public string FullName {
            get { return Type.FullName; }
        }

        public bool IsValid {
            get { return InputType != null && OutputType != null && Constructor != null; }
        }

        private Type[] GetGenericArguments(Type type) {
            while (type != null) {
                Type[] typeArguments = type.GetGenericArguments();
                if (typeArguments.Length >= 2) {
                    return typeArguments;
                } else {
                    type = type.BaseType;
                }
            }
            return null;
        }

        public PipeType(Type type) {
            Type = type;

            Type[] typeArguments = GetGenericArguments(Type);
            if (typeArguments == null || typeArguments.Length < 2) {
                Log.Error("Invalid Pipe Type: Invalid Generic Arguments: {0} -> {1}",
                            Type.FullName, typeArguments.Length);
                for (int i = 0; i < typeArguments.Length; i++) {
                    Log.Info("    {0} -> {1}", i, typeArguments[i].FullName);
                }
                return;
            }
            InputType = typeArguments[0];
            OutputType = typeArguments[1];

            ConstructorInfo[] constructors = Type.GetConstructors();
            if (constructors.Length != 1) {
                Log.Error("Invalid Pipe Type: Invalid Constructors: {0} -> {1}",
                            Type.FullName, constructors.Length);
                for (int i = 0; i < constructors.Length; i++) {
                    Log.Info("    {0} -> {1}", i, GetString(constructors[i]));
                }
                return;
            }

            ParameterInfo[] parameters = constructors[0].GetParameters();
            for (int i = 0; i < parameters.Length; i++) {
                ParameterInfo param = parameters[i];
                if (param.ParameterType != typeof(string)) {
                    Log.Error("Invalid Pipe Type: Invalid Constructors: {0} -> {1}",
                                Type.FullName, GetString(constructors[0]));
                    return;
                }
            }

            Constructor = constructors[0];
            ParamCount = parameters.Length;
        }

        public bool IsValidInputType(Type type) {
            return type != null && InputType.IsAssignableFrom(type);
        }

        public bool IsValidOutputType(Type type) {
            return type != null && OutputType.IsAssignableFrom(type);
        }

        public override string ToString() {
            return string.Format("[{0}, {1}, {2}, {3}, {4}]", FullName,
                    InputType == null ? "null" : InputType.FullName,
                    OutputType == null ? "null" : OutputType.FullName,
                    ParamCount,
                    Constructor == null ? "null" : GetString(Constructor)
                );
        }

        private string GetString(ConstructorInfo ctor) {
            StringBuilder builder = new StringBuilder();
            builder.Append("(");
            ParameterInfo[] parameters = ctor.GetParameters();
            for (int i = 0; i < parameters.Length; i++) {
                ParameterInfo param = parameters[i];
                builder.Append(string.Format("{0} {1}",
                                param.ParameterType.FullName, param.Name));
                if (i < parameters.Length - 1) {
                    builder.Append(",");
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        public IPipe CreateInstance(params string[] parameters) {
            if (Constructor == null) {
                return null;
            }
            if (parameters == null && ParamCount != 0) {
                return null;
            }
            if (parameters.Length != ParamCount) {
                return null;
            }
            object result = Constructor.Invoke(parameters);
            if (result is IPipe) {
                return (IPipe)result;
            }
            return null;
        }
    }
}

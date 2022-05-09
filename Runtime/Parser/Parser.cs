using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

using Edger.Unity;

namespace Edger.Unity.Glue {
    public static class ParserConsts {
        //Temp Scripts Won't be Cached
        public const string PrefixTempScriptPath = "_";

        private static int _NextTempId = 1;

        public static bool IsTempScriptPath(string path) {
            return path.StartsWith(PrefixTempScriptPath);
        }

        public static string TakeTempScriptPath() {
            return string.Format(PrefixTempScriptPath, _NextTempId++);
        }
    }

    public static class Parser {
        private static Dictionary<string, Dictionary<Type, PipeType>> _PipeTypes =
                            new Dictionary<string, Dictionary<Type, PipeType>>();

        private static Dictionary<string, IPipe> _PipeCache = new Dictionary<string, IPipe>();
        private static Dictionary<string, IScript> _Scripts = new Dictionary<string, IScript>();

        static Parser() {
            LoadPipeTypes();
        }

        private static void Clear() {
            _PipeCache.Clear();
            _Scripts.Clear();
        }

        public static void Reset() {
            Clear();
            LoadPipeTypes();
        }

        private static void LoadPipeTypes() {
            var startTime = DateTimeUtil.GetStartTime();
            AssemblyUtil.ForEachInterface<IPipe>(AddPipeType);
            Log.Info("LoadPipeTypes Finished: Took {0} Seconds", startTime.GetPassedSeconds());
        }

        private static void AddPipeType(Type type) {
            PipeType pipeType = new PipeType(type);
            if (!pipeType.IsValid) {
                Log.Error("Invalid Pipe Type: {0}", type.FullName);
                return;
            }
            Dictionary<Type, PipeType> types;
            if (!_PipeTypes.TryGetValue(pipeType.Name, out types)) {
                types = new Dictionary<Type, PipeType>();
                _PipeTypes[pipeType.Name] = types;
            }
            if (types.ContainsKey(pipeType.InputType)) {
                Log.Error("Pipe Type Conflicted: {0}, {1}: {2} -> {3}",
                        pipeType.Name, pipeType.InputType.FullName,
                        types[pipeType.InputType].FullName, pipeType.FullName);
                return;
            }
            types[pipeType.InputType] = pipeType;
            if (Log.LogDebug) {
                Log.Debug("Pipe Type Loaded: {0}, {1} -> {2}",
                            pipeType.Name, pipeType.InputType.FullName,
                            pipeType.Type.FullName);
            }
        }

        private static PipeType GetAssignablePipeType(PipeToken token, Type inputType,
                                    int paramCount, Dictionary<Type, PipeType> types) {
            PipeType result = null;
            var en = types.GetEnumerator();
            while (en.MoveNext()) {
                if (en.Current.Value.IsValidInputType(inputType)) {
                    if (result == null) {
                        result = en.Current.Value;
                    } else {
                        throw new GrammarException(token, "Multiple PipeType Found: {0}, {1}, {2} -> {3} -> {4}",
                                token.Name, inputType.FullName, paramCount, result, en.Current.Value);
                    }
                }
            }
            if (result == null) {
                throw new GrammarException(token, "Assignable PipeType Not Found: {0}, {1}, {2}",
                        token.Name, inputType.FullName, paramCount);
            }
            return result;
        }

        private static PipeType GetPipeType(PipeToken token, Type inputType, int paramCount) {
            PipeType result = null;
            Dictionary<Type, PipeType> types;
            if (_PipeTypes.TryGetValue(token.Name, out types)) {
                if (types.TryGetValue(inputType, out result)) {
                    if (result.ParamCount != paramCount) {
                        throw new GrammarException(token, "Mismatched Param Count: {0}, {1}, {2} -> {3}",
                                    token.Name, inputType.FullName, paramCount, result.ParamCount);
                    }
                }
                if (result == null) {
                    result = GetAssignablePipeType(token, inputType, paramCount, types);
                }
            } else {
                throw new GrammarException(token, "PipeType Not Found: {0}, {1}, {2}",
                        token.Name, inputType.FullName, paramCount);
            }
            return result;
        }

        private static string GetPipeCacheKey(Type inputType, string code) {
            return string.Format("{0}:{1}", inputType.FullName, code);
        }

        private static IPipe GetPipeFromCache(Type inputType, string code) {
            string pipeCacheKey = GetPipeCacheKey(inputType, code);
            IPipe result;
            if (_PipeCache.TryGetValue(pipeCacheKey, out result)) {
                return result;
            }
            return null;
        }

        private static bool SavePipeToCache(Type inputType, string code, IPipe pipe) {
            string pipeCacheKey = GetPipeCacheKey(inputType, code);
            if (!_PipeCache.ContainsKey(pipeCacheKey)) {
                _PipeCache[pipeCacheKey] = pipe;
                return true;
            }
            return false;
        }

        public static IScript GetScript(string path) {
            if (path == null) return null;
            IScript result;
            if (_Scripts.TryGetValue(path, out result)) {
                return result;
            }
            return null;
        }

        private static IPipe GetPipe(Pipeline pipeline, PipeToken token) {
            Type inputType = pipeline.GetNextInputType();

            string code = token.GetCode();
            IPipe pipe = GetPipeFromCache(inputType, code);

            if (pipe == null) {
                PipeType pipeType = GetPipeType(token, inputType, token.ParamCount);
                if (pipeType != null) {
                    try {
                        pipe = pipeType.CreateInstance(token.Params);
                    } catch (Exception e) {
                        throw new CodeException(token.Caret, e, "Failed to Create Pipe: {0} -> {1}", token, pipeType);
                    }
                    if (pipe != null) {
                        SavePipeToCache(inputType, code, pipe);
                    } else {
                        throw new CodeException(token.Caret, "Failed to Create Pipe: {0} -> {1}", token, pipeType);
                    }
                }
            } else {
                if (!pipe.IsValidInputType(inputType)) {
                    throw new CodeException(token.Caret, "Invalid Cached Pipe: {0}, {1} -> {2}", code, inputType.FullName, pipe);
                }
            }
            return pipe;
        }

        private static void AddPipeline(IToken token, Procedure proc, ref Pipeline pipeline) {
            if (proc == null || pipeline == null) {
                throw new GrammarException(token, "Broken Token Flow: {0}", token);
            } else {
                proc.AddPipeline(pipeline);
                pipeline = null;
            }
        }

        private static void AddProcedure(IToken token, Script script, ref Procedure proc, ref Pipeline pipeline) {
            if (proc == null) {
                throw new GrammarException(token, "Broken Token Flow: {0}", token);
            }
            if (pipeline != null) {
                AddPipeline(token, proc, ref pipeline);
            }
            if (proc != script.Init) {
                script.AddProcedure(proc);
            }
            proc = null;
        }

        private static Script ParseScript(string path, List<IToken> tokens) {
            if (tokens == null || tokens.Count == 0) {
                Log.Critical("ParseScript Failed: No Tokens: {0}", path);
                return null;
            }

            Script script = new Script(path, tokens[0].Caret);

            Procedure proc = script.Init;
            Pipeline pipeline = new Pipeline();

            foreach (var token in tokens) {
                if (token is SeparatorToken) {
                    switch (((SeparatorToken)token).Type) {
                        case SeparatorType.PipeSeparator:
                            if (pipeline == null) {
                                throw new GrammarException(token, "Broken Token Flow: {0}", token);
                            }
                            break;
                        case SeparatorType.PipelineSeparator:
                            AddPipeline(token, proc, ref pipeline);
                            pipeline = new Pipeline();
                            break;
                        case SeparatorType.ProcedureDeclaration:
                            if (proc == null) {
                                throw new GrammarException(token, "Broken Token Flow: {0}", token);
                            } else {
                                pipeline = new Pipeline();
                            }
                            break;
                    }
                } else if (token is PipeToken) {
                    if (pipeline == null) {
                        throw new GrammarException(token, "Broken Token Flow: {0}", token);
                    }
                    IPipe pipe = GetPipe(pipeline, (PipeToken)token);
                    if (pipe != null) {
                        pipeline.AddPipe(pipe, token.Caret);
                    }
                } else if (token is IdentityToken) {
                    AddProcedure(token, script, ref proc, ref pipeline);
                    proc = new Procedure(token.Caret, ((IdentityToken)token).Name);
                    pipeline = null;
                }
            }

            if (proc != null) {
                AddProcedure(null, script, ref proc, ref pipeline);
            }

            return script;
        }

        private static InvalidScript GetInvalidScript(string path, string content, Exception e) {
            Log.Error("Parse Failed: {0}\n{1}\n\n{2}",
                        CaretException.GetMessage(path, e),
                        e.StackTrace,
                        WordSplitter.AppendLineNumber(content));
            return new InvalidScript(path, content, e);
        }

        public static IScript ParseScript(string path, string content) {
            IScript script = null;

            bool isTemp = ParserConsts.IsTempScriptPath(path);

            if (!isTemp) {
                script = GetScript(path);
                if (script != null) {
                    //Not checking the content is same here.
                    return script;
                }
            }

            try {
                List<IToken> tokens = Tokenizer.Process(path, content);
                script = ParseScript(path, tokens);
            } catch (System.Reflection.TargetInvocationException e) {
                script = GetInvalidScript(path, content, e.InnerException);
            } catch (Exception e) {
                script = GetInvalidScript(path, content, e);
            }

            if (!isTemp) {
                _Scripts[path] = script;
            }
            return script;
        }

        public static IScript ParseTempScript(string content) {
            return ParseScript(ParserConsts.TakeTempScriptPath(), content);
        }
    }
}

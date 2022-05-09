using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using Edger.Unity;

namespace Edger.Unity.Glue {
    public static class TokenizerConsts {
        public const string ParamsBegin = "(";
        public const string ParamsEnd = ")";
        public const string ParamsSeparator = ",";

        public const string LineComment = "#";
        public const string BlockCommentBegin = "/*";
        public const string BlockCommentEnd = "*/";
 
        public readonly static char[] TokenChars = new char[]{'(', ')', ','};
    }

    public static class Tokenizer {
        private static void AddToken(List<IToken> tokens, IToken token) {
            tokens.Add(token);
        }

        private static PipeToken GetLastOpenPipeToken(List<IToken> tokens, Word word, bool throwException) {
            PipeToken lastPipeToken = null;
            if (tokens.Count > 0) {
                lastPipeToken = tokens[tokens.Count - 1] as PipeToken;
            }
            if (throwException) {
                if (lastPipeToken == null) {
                    throw new WordException(word, "Not Inside PipeTokon");
                }
            }
            return lastPipeToken;
        }

        private static void ProcessWord(List<IToken> tokens, Word word, Word nextWord) {
            if (word.Value == TokenizerConsts.ParamsBegin) {
                PipeToken lastPipeToken = GetLastOpenPipeToken(tokens, word, true);
                if (lastPipeToken.ParamCount > 0) {
                    throw new WordException(word, "PipeToken Already Has Param: {0}", lastPipeToken);
                }
            } else if (word.Value == TokenizerConsts.ParamsEnd) {
                PipeToken lastPipeToken = GetLastOpenPipeToken(tokens, word, true);
            } else if (SeparatorTypeConsts.IsSeparator(word.Value)) {
                AddToken(tokens, SeparatorToken.ToSeparatorToken(word));
            } else {
                PipeToken lastPipeToken = GetLastOpenPipeToken(tokens, word, false);
                if (lastPipeToken == null) {
                    if (nextWord != null) {
                        if (nextWord.Value == TokenizerConsts.ParamsBegin) {
                            AddToken(tokens, new PipeToken(word.Caret, word.Value));
                        } else if (nextWord.Value == SeparatorTypeConsts.ProcedureDeclaration) {
                            AddToken(tokens, new IdentityToken(word.Caret, word.Value));
                        }
                    }
                } else {
                    if (word.Value != TokenizerConsts.ParamsSeparator) {
                        lastPipeToken.AddParam(word.Value);
                    }
                }
            }
        }

        private static void ProcessWord(List<IToken> tokens, List<Word> words, int i, Word word,
                                        ref int lineCommenting, ref bool blockCommenting) {
            if (blockCommenting) {
                if (word.Value == TokenizerConsts.BlockCommentEnd) {
                    blockCommenting = false;
                }
            } else if (lineCommenting > 0) {
                if (word.Caret.Line > lineCommenting) {
                    lineCommenting = 0;
                    ProcessWord(tokens, words, i, word, ref lineCommenting, ref blockCommenting);
                }
            } else if (word.Value.StartsWith(TokenizerConsts.BlockCommentBegin)) {
                blockCommenting = true;
            } else if (word.Value.StartsWith(TokenizerConsts.LineComment)) {
                lineCommenting = word.Caret.Line;
            } else {
                Word nextWord = i < words.Count - 1 ? words[i + 1] : null;
                ProcessWord(tokens, word, nextWord);
            }
        }

        public static List<IToken> Process(string path, string content) {
            List<IToken> tokens = new List<IToken>();
            List<Word> words = WordSplitter.Split(path, content, TokenizerConsts.TokenChars);

            int lineCommenting = 0;
            bool blockCommenting = false;
            for (int i = 0; i < words.Count; i++) {
                Word word = words[i];
                ProcessWord(tokens, words, i, word, ref lineCommenting, ref blockCommenting);
            }
            return tokens;
        }
    }
}

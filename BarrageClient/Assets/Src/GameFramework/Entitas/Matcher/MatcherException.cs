using System;

namespace GameFramework
{

    public class MatcherException : GameFrameworkException {
        public MatcherException(int indices) : base(
            "matcher.indices.Length must be 1 but was " + indices) {
        }
    }
}

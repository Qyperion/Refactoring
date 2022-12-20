using System;

namespace Refactoring
{
    public class P
    {
        public Result P1 { get; set; }
        public Result P2 { get; set; }
        public TimeSpan D { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as P);
        }

        public bool Equals(P other)
        {
            return other != null
                   && (P1?.Equals(other.P1) ?? other.P1 == null)
                   && (P2?.Equals(other.P2) ?? other.P2 == null)
                   && D.Equals(other.D);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(P1, P2, D);
        }
    }
}
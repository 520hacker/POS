namespace Rpc.Internals
{
    using System;

    /// <summary>Parser context, we maintain contexts in a stack to avoiding recursion. </summary>
    internal struct Context
    {
        public String Name;
        public Object Container;
    }
}
using System;
using POS.Contracts;
using POS.Contracts.Architecture;

namespace TestPlugin
{
    [PlugIn("TestPlugin")]
    public class TestPlugin : PlugIn<IPosApplication>, IPosPlugIn
    {
        public void Load()
        {
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using HtmlAgilityPack;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = false)]
    public class XmlModule
    {
        [ScriptFunction(Name = "$")]
        public static IEnumerable<HtmlNode> Dollar(string content, string element)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);

            return document.DocumentNode.Descendants(element);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodeBuilder.Library;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void SimpleCase()
        {
            var items = new string[]
            {
                "this/that/other/hello.jpg",
                "this/that/another/goodbye.jpg",
                "this/willy/hello/thiska.docx",
                "yambo/that/other/whenever.txt",
                "yambo/yilma/hoopla/thalamus.json"
            };

            var node = items.ToNode((s) => s, '/');
            Assert.IsTrue(node.Children.SequenceEqual(new string[]
            {
                "this",
                "yambo"
            }));
        }
    }
}

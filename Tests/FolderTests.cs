using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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

            var folder = items.ToFolder((s) => s, '/');
            Assert.IsTrue(folder.Folders.Select(nd => nd.Name).SequenceEqual(new string[]
            {
                "this", "yambo"
            }));

            var json = JsonConvert.SerializeObject(folder);
        }

        [TestMethod]
        public void EvenSimplerCase()
        {
            var items = new string[]
            {
                "this/that/whatever.jpg",
                "this/that/yes.docx",
                "that/sorlag/samzo.json",
                "that/sorlag/no.txt"
            };

            var folder = items.ToFolder((s) => s);
            var json = JsonConvert.SerializeObject(folder);
        }
    }
}

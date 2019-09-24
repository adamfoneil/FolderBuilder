using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using FolderBuilder.Library;
using System.IO;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class FolderTests
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

            var folder = items.ToFolderStructure((s) => s);
            var json = JsonConvert.SerializeObject(folder, Formatting.Indented);
            Assert.AreEqual(GetEmbeddedResource("Tests.Resources.SimpleCase.json"), json);
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

            var folder = items.ToFolderStructure((s) => s);
            var json = JsonConvert.SerializeObject(folder, Formatting.Indented);            
            Assert.AreEqual(GetEmbeddedResource("Tests.Resources.EvenSimplerCase.json"), json);
        }

        [TestMethod]
        public void WithRootItems()
        {
            var items = new string[]
            {
                "this/that/whatever.jpg",
                "this/that/yes.docx",
                "that/sorlag/samzo.json",
                "that/sorlag/no.txt",
                "hapsburg",
                "shemlorian"
            };

            var folder = items.ToFolderStructure((s) => s);
            var json = JsonConvert.SerializeObject(folder, Formatting.Indented);
            Assert.AreEqual(GetEmbeddedResource("Tests.Resources.WithRootItems.json"), json);
        }

        private string GetEmbeddedResource(string name)
        {            
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

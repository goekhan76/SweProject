using TemperaturePlugin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TemperaturePluginTests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "TemperatureTest" und soll
    ///alle TemperatureTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class TemperatureTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Ruft den Testkontext auf, der Informationen
        ///über und Funktionalität für den aktuellen Testlauf bietet, oder legt diesen fest.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Zusätzliche Testattribute
        // 
        //Sie können beim Verfassen Ihrer Tests die folgenden zusätzlichen Attribute verwenden:
        //
        //Mit ClassInitialize führen Sie Code aus, bevor Sie den ersten Test in der Klasse ausführen.
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Mit ClassCleanup führen Sie Code aus, nachdem alle Tests in einer Klasse ausgeführt wurden.
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen.
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Ein Test für "pluginName"
        ///</summary>
        [TestMethod()]
        public void pluginNameTest()
        {
            Temperature target = new Temperature(); // TODO: Passenden Wert initialisieren
            string actual;
            actual = target.pluginName;
           // Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
            Assert.AreEqual("TemperaturePlugin", actual);
            Assert.IsNotNull(actual);
           
        }

        /// <summary>
        ///Ein Test für "openSQLCon"
        ///</summary>
        [TestMethod()]
        public void openSQLConTest()
        {
            Temperature target = new Temperature(); // TODO: Passenden Wert initialisieren
            bool expected = false; // TODO: Passenden Wert initialisieren
            bool actual;
            actual = target.openSQLCon();
            //Assert.AreEqual(expected, actual);
            
            Assert.IsTrue(target.openSQLCon());
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using dbm_lib.components;
using dbm_lib;
using DAFManager;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestArgumentWorker()
        {
            ArgumentsWorker aw = new ArgumentsWorker("");
            Assert.AreEqual("", aw.ToString(), "1");

            aw = new ArgumentsWorker("arg;argument=123");
            aw["arg"].Key = "arg2";
            Assert.AreEqual("arg2;argument=123", aw.ToString(), "2");

            Assert.ThrowsException<NullReferenceException>(() =>
            {
                aw["admin1"].Value = "123";
            }, "3");

            aw = new ArgumentsWorker("arg;argument=123");
            aw.Add("new_arg");
            aw.Add("nn_arg=43");

            Assert.AreEqual("arg;argument=123;new_arg;nn_arg=43", aw.ToString(), "4");

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                aw.Add("new_arg");
            }, "6");

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                aw.Add("new_arg=12");
            }, "7");
        }

        [TestMethod]
        public void MainTest()
        {
            MyTextBox mtb = new MyTextBox("Hello World!");
            Type t = mtb.GetType();
            if(t.GetCustomAttributes(typeof(TextColorAttribute), false).Length == 0)
            {
                Assert.AreEqual("[black] Hello World!", "[black] " + mtb.Text);
            }
            else
            {
                TextColorAttribute tca = t.GetCustomAttributes(typeof(TextColorAttribute), false)[0] as TextColorAttribute;
                Assert.AreEqual("[red] Hello World!", $"[{tca.Color}] {mtb.Text}");
            }
        }
    }
    class MyTextBox
    {
        public string Text { get; set; }

        public MyTextBox(string text)
        {
            Text = text;
        }
    }


    class TextColorAttribute : Attribute
    {
        public string Color { get; set; }

        public TextColorAttribute(string color)
        {
            Color = color;
        }
    }
}

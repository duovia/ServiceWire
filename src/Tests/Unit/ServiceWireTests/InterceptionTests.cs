﻿#region File Creator

// This File Created By Ersin Tarhan
// For Project : ServiceWire - ServiceWireTests
// On 2016 03 14 04:36

#endregion


#region Usings

using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ServiceWire.Aspects;

#endregion


namespace ServiceWireTests
{
    [TestClass]
    public class InterceptionTests
    {
        #region Methods


        #region Public Methods

        [TestMethod]
        public void SimpleTest()
        {
            var preInvokeInfo=string.Empty;
            var postInvokeInfo=string.Empty;
            var exceptionHandlerInfo=string.Empty;
            var cc=new CrossCuttingConcerns();
            cc.PreInvoke=(instanceId,methodName,parameters) => { preInvokeInfo=methodName+"_preInvokeInfo"; };
            cc.PostInvoke=(instanceId,methodName,parameters) => { postInvokeInfo=methodName+"_postInvokeInfo"; };
            cc.ExceptionHandler=(instanceId,methodName,parameters,exception) =>
            {
                exceptionHandlerInfo=methodName+"_exceptionHandlerInfo";
                return false; //do not throw
            };
            var t=Interceptor.Intercept<ISimpleMath>(new SimpleMath(),cc);
            var a=t.Add(1,2);
            Assert.IsFalse(string.IsNullOrEmpty(preInvokeInfo));
            Assert.IsFalse(string.IsNullOrEmpty(postInvokeInfo));
            Assert.IsTrue(string.IsNullOrEmpty(exceptionHandlerInfo));
            var b=t.Divide(4,0);
            Assert.IsFalse(string.IsNullOrEmpty(preInvokeInfo));
            Assert.IsFalse(string.IsNullOrEmpty(postInvokeInfo));
            Assert.IsFalse(string.IsNullOrEmpty(exceptionHandlerInfo));
        }

        [TestMethod]
        public void SimpleTimingTest()
        {
            var preInvokeInfo=string.Empty;
            var postInvokeInfo=string.Empty;
            var exceptionHandlerInfo=string.Empty;
            var cc=new CrossCuttingConcerns();
            cc.PreInvoke=(instanceId,methodName,parameters) => { preInvokeInfo=methodName+"_preInvokeInfo"; };
            cc.PostInvoke=(instanceId,methodName,parameters) => { postInvokeInfo=methodName+"_postInvokeInfo"; };
            cc.ExceptionHandler=(instanceId,methodName,parameters,exception) =>
            {
                exceptionHandlerInfo=methodName+"_exceptionHandlerInfo";
                return false; //do not throw
            };
            var t=Interceptor.Intercept<ISimpleMath>(new SimpleMath(),cc);
            var a=t.Add(1,2);
            Assert.IsFalse(string.IsNullOrEmpty(preInvokeInfo));
            Assert.IsFalse(string.IsNullOrEmpty(postInvokeInfo));
            Assert.IsTrue(string.IsNullOrEmpty(exceptionHandlerInfo));

            var sw=Stopwatch.StartNew();
            var t2=Interceptor.Intercept<ISimpleMath2>(new SimpleMath2(),cc);
            sw.Stop();
            var interceptCtorTicks=sw.ElapsedTicks;
            var interceptCtorTs=sw.Elapsed;
            sw.Reset();
            var c=t2.Add(2,3);
            sw.Stop();
            var interceptAddTicks=sw.ElapsedTicks;
            var interceptAddTs=sw.Elapsed;

            sw.Reset();
            var t3=Interceptor.Intercept<ISimpleMath2>(new SimpleMath2(),cc);
            sw.Stop();
            var interceptCtorTicks2=sw.ElapsedTicks;
            var interceptCtorTs2=sw.Elapsed;
            sw.Reset();
            var c2=t3.Add(2,3);
            sw.Stop();
            var interceptAddTicks2=sw.ElapsedTicks;
            var interceptAddTs2=sw.Elapsed;

            sw.Reset();
            var t4=new SimpleMath2();
            sw.Stop();
            var plainCtorTicks=sw.ElapsedTicks;
            var plainCtorTs=sw.Elapsed;
            sw.Reset();
            var c3=t4.Add(2,3);
            sw.Stop();
            var plainAddTicks=sw.ElapsedTicks;
            var plainAddTs=sw.Elapsed;

            Assert.IsTrue(plainCtorTicks<=interceptCtorTicks);
            Assert.IsTrue(plainAddTicks<=interceptAddTicks);

            Assert.IsTrue(plainCtorTicks<=interceptCtorTicks2);
            Assert.IsTrue(plainAddTicks<=interceptAddTicks2);

            Assert.IsTrue(plainCtorTs<=interceptCtorTs);
            Assert.IsTrue(plainAddTs<=interceptAddTs);

            Assert.IsTrue(plainCtorTs<=interceptCtorTs2);
            Assert.IsTrue(plainAddTs<=interceptAddTs2);
        }

        #endregion


        #endregion
    }

    public interface ISimpleMath
    {
        #region Methods


        #region Public Methods

        int Add(int a,int b);

        int Divide(int a,int b);

        #endregion


        #endregion
    }

    public class SimpleMath:ISimpleMath
    {
        #region Methods


        #region Public Methods

        public int Add(int a,int b)
        {
            return a+b;
        }

        public int Divide(int a,int b)
        {
            return a/b;
        }

        #endregion


        #endregion
    }

    public interface ISimpleMath2
    {
        #region Methods


        #region Public Methods

        int Add(int a,int b);

        int Divide(int a,int b);

        #endregion


        #endregion
    }

    public class SimpleMath2:ISimpleMath2
    {
        #region Methods


        #region Public Methods

        public int Add(int a,int b)
        {
            return a+b;
        }

        public int Divide(int a,int b)
        {
            return a/b;
        }

        #endregion


        #endregion
    }
}
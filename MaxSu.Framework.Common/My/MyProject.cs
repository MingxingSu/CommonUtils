using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.MyServices.Internal;

namespace MaxSu.Framework.Common.My
{
    [GeneratedCode("MyTemplate", "10.0.0.0"), HideModuleName, StandardModule]
    internal sealed class MyProject
    {
        private static readonly ThreadSafeObjectProvider<MyApplication> m_AppObjectProvider =
            new ThreadSafeObjectProvider<MyApplication>();

        private static readonly ThreadSafeObjectProvider<MyComputer> m_ComputerObjectProvider =
            new ThreadSafeObjectProvider<MyComputer>();

        private static readonly ThreadSafeObjectProvider<MyWebServices> m_MyWebServicesObjectProvider =
            new ThreadSafeObjectProvider<MyWebServices>();

        private static readonly ThreadSafeObjectProvider<User> m_UserObjectProvider =
            new ThreadSafeObjectProvider<User>();

        [HelpKeyword("My.Application")]
        internal static MyApplication Application
        {
            [DebuggerHidden] get { return m_AppObjectProvider.GetInstance; }
        }

        [HelpKeyword("My.Computer")]
        internal static MyComputer Computer
        {
            [DebuggerHidden] get { return m_ComputerObjectProvider.GetInstance; }
        }

        [HelpKeyword("My.User")]
        internal static User User
        {
            [DebuggerHidden] get { return m_UserObjectProvider.GetInstance; }
        }

        [HelpKeyword("My.WebServices")]
        internal static MyWebServices WebServices
        {
            [DebuggerHidden] get { return m_MyWebServicesObjectProvider.GetInstance; }
        }

        [MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__",
            "Dispose__Instance__", ""), EditorBrowsable(EditorBrowsableState.Never)]
        internal sealed class MyWebServices
        {
            [DebuggerHidden]
            private static T Create__Instance__<T>(T instance) where T : new()
            {
                if (instance == null)
                {
                    return Activator.CreateInstance<T>();
                }
                return instance;
            }

            [DebuggerHidden]
            private void Dispose__Instance__<T>(ref T instance)
            {
                instance = default(T);
            }

            [EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
            public override bool Equals(object o)
            {
                return base.Equals(RuntimeHelpers.GetObjectValue(o));
            }

            [EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            [EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
            internal Type GetType()
            {
                return typeof (MyWebServices);
            }

            [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
            public override string ToString()
            {
                return base.ToString();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), ComVisible(false)]
        internal sealed class ThreadSafeObjectProvider<T> where T : new()
        {
            private readonly ContextValue<T> m_Context;

            [EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
            public ThreadSafeObjectProvider()
            {
                m_Context = new ContextValue<T>();
            }

            internal T GetInstance
            {
                [DebuggerHidden]
                get
                {
                    T local2 = m_Context.Value;
                    if (local2 == null)
                    {
                        local2 = Activator.CreateInstance<T>();
                        m_Context.Value = local2;
                    }
                    return local2;
                }
            }
        }
    }
}
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MaxSu.Framework.Common
{
    // By implementing IDisposable, you are announcing that instances of this type allocate scarce resources. 

    /*Summary:
     * 1、 Finalize只释放非托管资源；
     * 2、 Dispose释放托管和非托管资源；
     * 3、 重复调用Finalize和Dispose是没有问题的；
     * 4、 Finalize和Dispose共享相同的资源释放策略，因此他们之间也是没有冲突的。
     */

    public class DisposableResource : IDisposable
    {
        //Unmanaged

        //Managed
        private readonly Component compoent = new Component();

        // Track whether Dispose has been called. 
        private bool disposed;
        private IntPtr handle;

        public DisposableResource(IntPtr handle)
        {
            this.handle = handle;
        }

        //True: user code calls, dispose both
        //False: Finalizer calls, dispose only unmanged
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Clean managed resource
                    compoent.Dispose();
                }
            }

            //Clean unmanaged resource
            CloseHandle(handle);
            handle = IntPtr.Zero;

            disposed = true;
        }

        [DllImport("Kernel32")]
        private static extern Boolean CloseHandle(IntPtr handle);

        ~DisposableResource()
        {
            Dispose(false);
        }

        //Close <=> Dispose
        public void Close()
        {
            Dispose();
        }

        #region IDisposable

        //DO NOT virtual this method.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
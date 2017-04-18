/// <summary>
/// 
/// </summary>
namespace NetOnlyImpersonation
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Security.Principal;

    /// <summary>
    /// This module allows using windows identity impersonation to access protected resources on remote systems as a different user without any trust. 
    /// Credentials are only used on the remote system, locally you are still using the identity of the process.
    /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/aa378184(v=vs.85).aspx"/>
    /// <see cref="https://technet.microsoft.com/en-us/library/bb490994.aspx"/>
    /// </summary>
    public class NetOnlyImpersonationContext : IDisposable
    {
        #region Interop

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        #endregion

        #region Const

        private const string LOCAL_ACCOUNT_DATABASE = ".";
        private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
        private const int LOGON32_PROVIDER_WINNT50 = 3;

        #endregion

        private WindowsImpersonationContext _context = null;
        private WindowsIdentity _identity = null;
        private IntPtr _token = IntPtr.Zero;
        private bool _disposed = false;

        public WindowsIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public NetOnlyImpersonationContext(string username, string password)
            : this(LOCAL_ACCOUNT_DATABASE, username, password)
        {
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public NetOnlyImpersonationContext(string domain, string username, string password)
        {
            var hresult = LogonUser(username, domain, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_WINNT50, ref _token);
            if (hresult == false)
                throw new Win32Exception(error: Marshal.GetLastWin32Error());

            _identity = new WindowsIdentity(_token);
            _context = _identity.Impersonate();
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_identity != null)
                    {
                        _identity.Dispose();
                        _identity = null;
                    }
                }

                if (_context != null)
                {
                    _context.Undo();
                    _context.Dispose();
                    _context = null;
                }

                if (_token != IntPtr.Zero)
                {
                    CloseHandle(_token);
                    _token = IntPtr.Zero;
                }

                _disposed = true;
            }
        }

        ~NetOnlyImpersonationContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

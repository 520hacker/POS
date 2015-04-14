using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Pos.Internals.Extensions
{
    /// <summary>
    /// Extracts the shell thumbnail from a file.
    /// </summary>
    public class ShellThumbnail : IDisposable
    {
        /// <summary>
        /// IMalloc instance.
        /// </summary>
        private IMalloc alloc = null;

        /// <summary>
        /// Denotes whether the instance has been disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Desired thumbnail size.
        /// </summary>
        private Size desiredSize = new Size(100, 100);

        /// <summary>
        /// Finalizes an instance of the ShellThumbnail class.
        /// </summary>
        ~ShellThumbnail()
        {
            this.Dispose();
        }

        #region Enums

        /// <summary>
        /// ESTRRET flags.
        /// </summary>
        [Flags]
        private enum ESTRRET
        {
        }

        /// <summary>
        /// ESHCONTF flags.
        /// </summary>
        [Flags]
        private enum ESHCONTF
        {
            SHCONTF_FOLDERS = 32,
            SHCONTF_NONFOLDERS = 64,
        }

        /// <summary>
        /// ESHGDN flags.
        /// </summary>
        [Flags]
        private enum ESHGDN
        {
        }

        /// <summary>
        /// ESFGAO flags.
        /// </summary>
        [Flags]
        private enum ESFGAO
        {
        }

        /// <summary>
        /// EIEIFLAG flags.
        /// </summary>
        private enum EIEIFLAG
        {
            IEIFLAG_ASPECT = 4,
            IEIFLAG_SCREEN = 32,
        }

        #endregion

        #region Interfaces

        /// <summary>
        /// IMalloc interface.
        /// </summary>
        [ComImport()]
        [Guid("00000002-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMalloc
        {
            /// <summary>
            /// Allocates memory.
            /// </summary>
            /// <param name="cb">Integer parameter.</param>
            /// <returns>Resulting pointer.</returns>
            [PreserveSig()]
            IntPtr Alloc(int cb);

            /// <summary>
            /// Reallocates memory.
            /// </summary>
            /// <param name="pv">Pointer to reallocate.</param>
            /// <param name="cb">Integer parameter.</param>
            /// <returns>Resulting pointer.</returns>
            [PreserveSig()]
            IntPtr Realloc(IntPtr pv, int cb);

            /// <summary>
            /// Frees memory.
            /// </summary>
            /// <param name="pv">Pointer to free.</param>
            [PreserveSig()]
            void Free(IntPtr pv);

            /// <summary>
            /// Gets size of pointer.
            /// </summary>
            /// <param name="pv">Pointer to inspect.</param>
            /// <returns>Resulting size.</returns>
            [PreserveSig()]
            int GetSize(IntPtr pv);

            /// <summary>
            /// Detects allocation.
            /// </summary>
            /// <param name="pv">Pointer to inspect.</param>
            /// <returns>Resulting integer.</returns>
            [PreserveSig()]
            int DidAlloc(IntPtr pv);

            /// <summary>
            /// Minimizes heap.
            /// </summary>
            [PreserveSig()]
            void HeapMinimize();
        }

        /// <summary>
        /// IEnumIDList interface.
        /// </summary>
        [ComImport()]
        [Guid("000214F2-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IEnumIDList
        {
            /// <summary>
            /// Navigates to the next ID.
            /// </summary>
            /// <param name="celt">Integer parameter.</param>
            /// <param name="rgelt">Pointer parameter.</param>
            /// <param name="pceltFetched">Amount fetched.</param>
            /// <returns>Resulting integer.</returns>
            [PreserveSig()]
            int Next(int celt, ref IntPtr rgelt, ref int pceltFetched);
        }

        /// <summary>
        /// IShellFolder interface.
        /// </summary>
        [ComImport()]
        [Guid("000214E6-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellFolder
        {
            /// <summary>
            /// Parses the display name.
            /// </summary>
            /// <param name="hwndOwner">Pointer parameter.</param>
            /// <param name="pbcReserved">Pointer parameter.</param>
            /// <param name="lpszDisplayName">Display name.</param>
            /// <param name="pchEaten">Integer parameter.</param>
            /// <param name="ppidl">Pointer parameter.</param>
            /// <param name="pdwAttributes">Integer parameter.</param>
            void ParseDisplayName(IntPtr hwndOwner, IntPtr pbcReserved, [MarshalAs(UnmanagedType.LPWStr)]
                                  string lpszDisplayName, ref int pchEaten, ref IntPtr ppidl, ref int pdwAttributes);

            /// <summary>
            /// Enumerates objects.
            /// </summary>
            /// <param name="hwndOwner">Handle owner.</param>
            /// <param name="grfFlags">Flags parameter.</param>
            /// <param name="ppenumIDList">ID list parameter.</param>
            void EnumObjects(IntPtr hwndOwner, [MarshalAs(UnmanagedType.U4)]
                             ESHCONTF grfFlags, ref IEnumIDList ppenumIDList);

            /// <summary>
            /// Binds to an object.
            /// </summary>
            /// <param name="pidl">ID parameter.</param>
            /// <param name="pbcReserved">Reserved parameter.</param>
            /// <param name="riid">Globally unique ID.</param>
            /// <param name="ppvOut">Shell folder.</param>
            void BindToObject(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, ref IShellFolder ppvOut);

            [PreserveSig()]
            int CompareIDs(IntPtr compareParam, IntPtr pidl1, IntPtr pidl2);

            void GetUIObjectOf(IntPtr hwndOwner, int cidl, ref IntPtr apidl, ref Guid riid, ref int prgfInOut, ref IUnknown ppvOut);
        }

        /// <summary>
        /// IExtractImage interface.
        /// </summary>
        [ComImport(), Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IExtractImage
        {
            void GetLocation([Out(), MarshalAs(UnmanagedType.LPWStr)]
                             StringBuilder pathBuffer, int cch, ref int extractPriority, ref SIZE extractSize, int colorDepth, ref int extractFlags);

            void Extract(ref IntPtr bitmapThumbnail);
        }

        /// <summary>
        /// IUnknown interface.
        /// </summary>
        [ComImport(), Guid("00000000-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IUnknown
        {
            [PreserveSig()]
            IntPtr QueryInterface(ref Guid riid, ref IntPtr voidPointer);

            [PreserveSig()]
            IntPtr AddRef();

            [PreserveSig()]
            IntPtr Release();
        }

        #endregion

        #region Properties

        #region Public Properties

        public Size DesiredSize
        {
            get
            {
                return this.desiredSize;
            }
            set
            {
                this.desiredSize = value;
            }
        }

        public Bitmap ThumbNail { get; private set; }

        #endregion

        #region Private Properties

        private IMalloc Allocator
        {
            get
            {
                if (!this.disposed)
                {
                    if (this.alloc == null)
                    {
                        UnmanagedMethods.SHGetMalloc(ref this.alloc);
                    }
                }
                else
                {
                    Debug.Assert(false, "Object has been disposed.");
                }

                return this.alloc;
            }
        }

        private IShellFolder DesktopFolder
        {
            get
            {
                IShellFolder ppshf = null;

                int r = UnmanagedMethods.SHGetDesktopFolder(ref ppshf);

                return ppshf;
            }
        }

        #endregion

        #endregion

        #region Public Methods

        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.alloc != null)
                {
                    Marshal.ReleaseComObject(this.alloc);
                }

                this.alloc = null;

                if (this.ThumbNail != null)
                {
                    this.ThumbNail.Dispose();
                }

                this.disposed = true;
            }
        }

        public Bitmap GetThumbnail(string fileName)
        {
            if (!File.Exists(fileName) && !Directory.Exists(fileName))
            {
                throw new FileNotFoundException(string.Format("The file '{0}' does not exist", fileName), fileName);
            }

            if (this.ThumbNail != null)
            {
                this.ThumbNail.Dispose();
                this.ThumbNail = null;
            }

            IShellFolder folder = null;

            try
            {
                folder = this.DesktopFolder;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (folder != null)
            {
                IntPtr pidlMain = IntPtr.Zero;

                try
                {
                    int parsed = 0;
                    int pdwAttrib = 0;
                    string filePath = Path.GetDirectoryName(fileName);

                    folder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, filePath, ref parsed, ref pidlMain, ref pdwAttrib);
                }
                catch (Exception ex)
                {
                    Marshal.ReleaseComObject(folder);

                    throw ex;
                }

                if (pidlMain != IntPtr.Zero)
                {
                    Guid iidShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");

                    IShellFolder item = null;

                    try
                    {
                        folder.BindToObject(pidlMain, IntPtr.Zero, ref iidShellFolder, ref item);
                    }
                    catch (Exception ex)
                    {
                        Marshal.ReleaseComObject(folder);

                        this.Allocator.Free(pidlMain);

                        throw ex;
                    }

                    if (item != null)
                    {
                        IEnumIDList idEnum = null;

                        try
                        {
                            item.EnumObjects(IntPtr.Zero, (ESHCONTF.SHCONTF_FOLDERS | ESHCONTF.SHCONTF_NONFOLDERS), ref idEnum);
                        }
                        catch (Exception ex)
                        {
                            Marshal.ReleaseComObject(folder);

                            this.Allocator.Free(pidlMain);

                            throw ex;
                        }

                        if (idEnum != null)
                        {
                            int res = 0;
                            IntPtr pidl = IntPtr.Zero;
                            int fetched = 0;
                            bool complete = false;

                            while (!complete)
                            {
                                res = idEnum.Next(1, ref pidl, ref fetched);

                                if (res != 0)
                                {
                                    pidl = IntPtr.Zero;
                                    complete = true;
                                }
                                else
                                {
                                    if (this.GetThumbnailHelper(fileName, pidl, item))
                                    {
                                        complete = true;
                                    }
                                }

                                if (pidl != IntPtr.Zero)
                                {
                                    this.Allocator.Free(pidl);
                                }
                            }

                            Marshal.ReleaseComObject(idEnum);
                        }

                        Marshal.ReleaseComObject(item);
                    }

                    this.Allocator.Free(pidlMain);
                }

                Marshal.ReleaseComObject(folder);
            }

            return this.ThumbNail;
        }

        #endregion

        #region Private Methods

        private bool GetThumbnailHelper(string file, IntPtr pidl, IShellFolder item)
        {
            IntPtr bitmapPointer = IntPtr.Zero;

            IExtractImage extractImage = null;

            try
            {
                string pidlPath = this.PathFromPidl(pidl);

                if (Path.GetFileName(pidlPath).ToUpper().Equals(Path.GetFileName(file).ToUpper()))
                {
                    IUnknown iunk = null;
                    int prgf = 0;
                    Guid iidExtractImage = new Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1");
                    item.GetUIObjectOf(IntPtr.Zero, 1, ref pidl, ref iidExtractImage, ref prgf, ref iunk);
                    extractImage = (IExtractImage)iunk;
                    if (extractImage != null)
                    {
                        Console.WriteLine(string.Format("Got an IExtractImage object - {0}", file));
                        SIZE sz = new SIZE();
                        sz.HorizontalSize = this.DesiredSize.Width;
                        sz.VerticalSize = this.DesiredSize.Height;
                        StringBuilder location = new StringBuilder(260, 260);
                        int priority = 0;
                        int requestedColourDepth = 32;
                        EIEIFLAG flags = EIEIFLAG.IEIFLAG_ASPECT | EIEIFLAG.IEIFLAG_SCREEN;
                        int nameFlags = (int)flags;
                        extractImage.GetLocation(location, location.Capacity, ref priority, ref sz, requestedColourDepth, ref nameFlags);
                        extractImage.Extract(ref bitmapPointer);

                        if (bitmapPointer != IntPtr.Zero)
                        {
                            this.ThumbNail = Bitmap.FromHbitmap(bitmapPointer);
                        }

                        Marshal.ReleaseComObject(extractImage);

                        extractImage = null;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (bitmapPointer != IntPtr.Zero)
                {
                    UnmanagedMethods.DeleteObject(bitmapPointer);
                }

                if (extractImage != null)
                {
                    Marshal.ReleaseComObject(extractImage);
                }

                throw ex;
            }
        }

        private string PathFromPidl(IntPtr pidl)
        {
            StringBuilder path = new StringBuilder(260, 260);

            int result = UnmanagedMethods.SHGetPathFromIDList(pidl, path);

            if (result == 0)
            {
                return string.Empty;
            }
            else
            {
                return path.ToString();
            }
        }

        #endregion

        #region Structs

        /// <summary>
        /// STRRET structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0, CharSet = CharSet.Auto)]
        private struct STRRET_CSTR
        {
            /// <summary>
            /// ESTRRET string.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 520)]
            public byte[] Str;
        }

        /// <summary>
        /// STRRET struct.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
        private struct STRRET_ANY
        {
            /// <summary>
            /// ESTRRET type.
            /// </summary>
            [FieldOffset(0)]
            public ESTRRET Type;

            /// <summary>
            /// OLE string.
            /// </summary>
            [FieldOffset(4)]
            public IntPtr OLEString;
        }

        /// <summary>
        /// SIZE struct.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct SIZE
        {
            /// <summary>
            /// Horizontal size.
            /// </summary>
            public int HorizontalSize;

            /// <summary>
            /// Vertical size.
            /// </summary>
            public int VerticalSize;
        }

        #endregion

        /// <summary>
        /// Unmanaged methods.
        /// </summary>
        private class UnmanagedMethods
        {
            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal static extern int SHGetMalloc(ref IMalloc malloc);

            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal static extern int SHGetDesktopFolder(ref IShellFolder ppshf);

            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal static extern int SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);

            [DllImport("gdi32", CharSet = CharSet.Auto)]
            internal static extern int DeleteObject(IntPtr obj);
        }
    }
}
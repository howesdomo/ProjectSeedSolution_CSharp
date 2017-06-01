using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Util.ZPLPrinter
{
    public class ZPLHelper
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docName"></param>
        /// <param name="szPrinterName"></param>
        /// <param name="pBytes"></param>
        /// <param name="dwCount"></param>
        /// <returns></returns>
        private static bool sendBytesToPrinter(string docName, string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0;
            Int32 dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA docInfo = new DOCINFOA();
            bool bResult = false; // Assume failure unless you specifically succeed.

            docInfo.pDocName = docName;
            docInfo.pDataType = "RAW";

            if (ZPLHelper.OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                if (ZPLHelper.StartDocPrinter(hPrinter, 1, docInfo))
                {
                    if (ZPLHelper.StartPagePrinter(hPrinter))
                    {
                        bResult = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        ZPLHelper.EndPagePrinter(hPrinter);
                    }
                    ZPLHelper.EndDocPrinter(hPrinter);
                }
                ZPLHelper.ClosePrinter(hPrinter);
            }
            if (bResult == false)
            {
                dwError = Marshal.GetLastWin32Error(); // If you did not succeed, GetLastError may give more information about why not.
            }
            return bResult;
        }


        public static bool SendFileToPrinter(string docName, string szPrinterName, string szFileName)
        {
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = new Byte[fs.Length];
            bool bResult = false;
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            bytes = br.ReadBytes(nLength);
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            bResult = ZPLHelper.sendBytesToPrinter(docName, szPrinterName, pUnmanagedBytes, nLength);
            Marshal.FreeCoTaskMem(pUnmanagedBytes); // Free the unmanaged memory that you allocated earlier.
            return bResult;
        }

        /// <summary>
        /// Send the converted ANSI string to the printer.
        /// 打印中文需要指定字符集信息
        /// </summary>
        /// <param name="szPrinterName"></param>
        /// <param name="szString"></param>
        /// <returns></returns>
        public static bool SendStringToPrinter(string docName, string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = StringHelper.CountStringLength(szString);
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            ZPLHelper.sendBytesToPrinter(docName, szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

        /// <summary>
        /// <para>打印含有中文的ZPL II条码</para>
        /// <para>使用此方法可以无需放置字符集（GB2312, GB18080等）到打印机 或 指定字符集</para>
        /// <para>ZPLII 指令事例</para>
        /// ^XA
        /// ^CW1,E:MSHEI.FNT -- 指定打印机的中文字体
        /// ^CI28            -- 
        /// ^LH0,0
        /// ^FO20,20^A1N,25,25^FD大中華排名地下城(アテナ杯)開催決定！ ^FS^XZ"
        /// </summary>
        /// <param name="docName">打印文档名称</param>
        /// <param name="printerName">指定打印机名称</param>
        /// <param name="szString">ZPL II 指令</param>
        /// <returns></returns>
        public static bool SendChineseStringToPrinter(string docName, string printerName, string szString)
        {
            bool bSuccess = false;
            byte[] byteArray = Encoding.UTF8.GetBytes(szString);
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength = byteArray.Length;
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            Marshal.Copy(byteArray, 0, pUnmanagedBytes, nLength);
            bSuccess = ZPLHelper.sendBytesToPrinter(docName, printerName, pUnmanagedBytes, nLength);
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }
    }
}
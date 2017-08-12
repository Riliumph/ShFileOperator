using System;
using System.Runtime.InteropServices;

namespace WinApiUtil
{
	/// <summary>
	/// SHFileOperationの処理クラス
	/// </summary>
	public partial class SHFileOperator
	{
		#region DefineWinApi
		internal partial class NativeMethod
		{
			[DllImport( "shell32.dll", CharSet = CharSet.Unicode )]
			public static extern int SHFileOperation( [In] ref OperationInfo lpFileOp );
		}

		public enum Operations : uint
		{
			MOVE = 0x0001,
			COPY = 0x0002,
			DELETE = 0x0003,
			RENAME = 0x0004,
		}

		/// <summary>
		/// 実行時にAPIへ渡す構造体の定義
		/// </summary>
		public struct OperationInfo
		{
			public IntPtr hwnd;
			public Operations wFunc;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pFrom;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pTo;
			public ushort fFlags;
			public bool fAnyOperationsAborted;
			public IntPtr hNameMappings;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszProgressTitle;
		}
		#endregion
	}
}

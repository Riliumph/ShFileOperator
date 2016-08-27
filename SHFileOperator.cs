using System;
using System.Runtime.InteropServices;

namespace WinApiUtil
{
	/// <summary>
	/// SHFileOperationの処理クラス
	/// </summary>
	public class SHFileOperator
	{
		#region DefineWinApi
		internal partial class NativeMethod
		{
			[DllImport( "shell32.dll", CharSet = CharSet.Unicode )]
			public static extern int SHFileOperation( [In] ref Info lpFileOp );
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
		public struct Info
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

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SHFileOperator()
		{
			_ShFile.hwnd = IntPtr.Zero;
			_ShFile.wFunc = Operations.COPY;
			_ShFile.pFrom = "";
			_ShFile.pTo = "";
			_ShFile.fAnyOperationsAborted = false;
			_ShFile.hNameMappings = IntPtr.Zero;
			_ShFile.lpszProgressTitle = "";
		}


		/// <summary>
		/// 実行関数
		/// </summary>
		/// <returns></returns>
		public int Execute()
		{
			_ShFile.fFlags = Config.FlagsToBit();
			return NativeMethod.SHFileOperation( ref _ShFile );
		}

		#region Fields
		/// <summary>
		/// SHFileOperation関数へ渡される構造体のインスタンス
		/// ushortのビットフラグを公開したくないので、インスタンスはprivateにして各種アクセサを用意します。
		/// </summary>
		private Info _ShFile = new Info();

		/// <summary>
		/// SHFileOperationの詳細設定フラグをまとめたクラスインスタンス
		/// 中身はすべて公開されているので、
		/// </summary>
		public Detail Config { get; } = new Detail();

		/// <summary>
		/// ウィンドウハンドル
		/// </summary>
		public IntPtr WindowHandle { set { _ShFile.hwnd = value; } }

		/// <summary>
		/// 処理モード
		/// </summary>
		public Operations Mode { set { _ShFile.wFunc = value; } }

		/// <summary>
		/// 処理From
		/// 末尾の"\0\0"を自動付与する。
		/// フルパスでないファイル名が指定された場合、カレントフォルダと見做されます。
		/// </summary>
		public string pFrom { set { _ShFile.pFrom = value + '\0' + '\0'; } }

		/// <summary>
		/// 処理To
		/// 末尾の"\0\0"を自動付与する。
		/// </summary>
		public string pTo { set { _ShFile.pTo = value + '\0' + '\0'; } }

		/// <summary>
		/// 指定したファイル操作が完了する前にユーザーによって中止されたことを示します。
		/// </summary>
		public bool IsAborted { get { return _ShFile.fAnyOperationsAborted; } }

		/// <summary>
		/// 移動・コピー・名前変更されたファイルの古いファイル名と新しいファイル名を含むファイル名マッピングオブジェクトのハンドルが格納されます。
		/// このメンバは fFlags メンバに FOF_WANTMAPPINGHANDLE フラグが指定された場合にのみ使用されます。
		/// このハンドルが不要になったら SHFreeNameMappings 関数で解放しなくてはなりません。
		/// </summary>
		public IntPtr NameMappingsHandle { get { return _ShFile.hNameMappings; } }

		/// <summary>
		/// タイトルバーの表示する文言。
		/// 末尾の"\0"を自動付与する。
		/// </summary>
		public string TitleBarName { set { _ShFile.lpszProgressTitle = value + '\0'; } }
		#endregion


		/// <summary>
		/// 
		/// </summary>
		public class Detail
		{
			private enum ConfigFlag : ushort
			{
				FOF_MULTIDESTFILES = 0x0001,    // The pTo member specifies multiple destination files (one for each source file) rather than one directory where all source files are to be deposited.
				FOF_CONFIRMMOUSE = 0x0002,      // Not used.
				FOF_SILENT = 0x0004,            // Don't display a progress dialog box.
				FOF_RENAMEONCOLLISION = 0x0008, // Give the file being operated on a new name in a move, copy, or rename operation if a file with the target name already exists.
				FOF_NOCONFIRMATION = 0x0010,    // 
				FOF_WANTMAPPINGHANDLE = 0x0020, // If FOF_RENAMEONCOLLISION is specified and any files were renamed, assign a name mapping object containing their old and new names to the hNameMappings member.
				FOF_ALLOWUNDO = 0x0040,
				FOF_FILESONLY = 0x0080,         // Perform the operation on files only if a wildcard file name (*.*) is specified.
				FOF_SIMPLEPROGRESS = 0x0100,    // Display a progress dialog box but do not show the file names.
				FOF_NOCONFIRMMKDIR = 0x0200,    // Don't confirm the creation of a new directory if the operation requires one to be created.
				FOF_NOERRORUI = 0x0400,         // Do not display a user interface if an error occurs.
				FOF_NOCOPYSECURITYATTRIBS = 0x0800, // Do not copy the security attributes of the file.
				FOF_NORECURSION = 0x1000,           // Only operate in the local directory. Don't operate recursively into subdirectories.
				FOF_NO_CONNECTED_ELEMENTS = 0x2000, // Do not move connected files as a group. Only move the specified files.
				FOF_WANTNUKEWARNING = 0x4000,
				FOF_NORECURSEREPARSE = 0x8000,
			}

			#region Fields
			/// <summary>
			/// pToメンバに単一のフォルダパスではなく、複数のファイルパスを渡すことを許可するフラグ。
			/// 複数のファイルを別々のパスへ処理したり、リネームコピーやリネームムーヴの際に立ち上げます。
			/// </summary>
			public bool FOF_MULTIDESTFILES = false;

			private readonly bool FOF_CONFIRMMOUSE = false;

			/// <summary>
			/// 進捗表示を表示するフラグ
			/// </summary>
			public bool FOF_SILENT = false;

			/// <summary>
			/// 名前衝突フラグ
			/// 処理中にファイル名が衝突した場合に、サフィックスを付与することを許可します。
			/// </summary>
			public bool FOF_RENAMEONCOLLISION = false;

			/// <summary>
			/// 処理中のユーザー操作ダイアログに対して、すべて「Yes」を強制選択するフラグ
			/// </summary>
			public bool FOF_NOCONFIRMATION = false;

			/// <summary>
			/// 名前衝突フラグが立っている場合、リネームしたファイル名をマッピングするフラグ。
			/// </summary>
			public bool FOF_WANTMAPPINGHANDLE = false;

			/// <summary>
			/// 出来る限り、アンドゥ情報を保存するフラグ
			/// pFromのパスにファイル名が含まれていない場合、このフラグは無視されます。
			/// pFromのパスがフルパスでないファイル名の場合、このフラグを指定してもゴミ箱へ移動しません。
			/// </summary>
			public bool FOF_ALLOWUNDO = false;

			/// <summary>
			/// ファイル処理限定フラグ
			/// ワイルドカードのファイル名（*.*）が指定されている場合にのみ、ファイルの操作を実行します。
			/// </summary>
			public bool FOF_FILESONLY = false;

			/// <summary>
			/// ファイル名表示フラグ。
			/// ダイアログ上に、処理中のファイル名を表示するフラグ
			/// </summary>
			public bool FOF_SIMPLEPROGRESS = false;

			/// <summary>
			/// フォルダ作成時の無確認フラグ。
			/// 新しいフォルダが作られる場合にユーザー確認を行いません。
			/// </summary>
			public bool FOF_NOCONFIRMMKDIR = false;

			/// <summary>
			/// エラー非表示フラグ。
			/// エラーが発生してもダイアログボックスを表示しません。
			/// </summary>
			public bool FOF_NOERRORUI = false;

			/// <summary>
			/// セキュリティ属性をコピーしません。
			/// </summary>
			public bool FOF_NOCOPYSECURITYATTRIBS = false;

			/// <summary>
			/// 非再帰走査フラグ。
			/// 指定されたフォルダのみを走査フォルダとし、サブフォルダへの再帰走査を行いません。
			/// </summary>
			public bool FOF_NORECURSION = false;

			/// <summary>
			///	グループになっているファイルを移動せず、指定したファイルのみを移動します。
			///	グループなっているファイル＝フォルダ名が「ファイル名.files」など
			/// </summary>
			public bool FOF_NO_CONNECTED_ELEMENTS = false;

			/// <summary>
			/// ファイル削除時の警告フラグ。
			/// 削除操作（ゴミ箱を介さない）の際に警告します。
			/// </summary>
			public bool FOF_WANTNUKEWARNING = false;

			/// <summary>
			///リパースポイントをコンテナではなくオブジェクトとして扱います。
			/// </summary>
			public bool FOF_NORECURSEREPARSE = false;
			#endregion

			/// <summary>
			/// boolフラグ群をビットフラグへ変換します。
			/// </summary>
			public ushort FlagsToBit()
			{
				ushort BitFlag = 0;
				if ( FOF_MULTIDESTFILES == true )
					BitFlag |= (ushort)ConfigFlag.FOF_MULTIDESTFILES;
				if ( FOF_CONFIRMMOUSE )
					BitFlag |= (ushort)ConfigFlag.FOF_CONFIRMMOUSE;
				if ( FOF_SILENT == true )
					BitFlag |= (ushort)ConfigFlag.FOF_SILENT;
				if ( FOF_RENAMEONCOLLISION == true )
					BitFlag |= (ushort)ConfigFlag.FOF_RENAMEONCOLLISION;
				if ( FOF_NOCONFIRMATION == true )
					BitFlag |= (ushort)ConfigFlag.FOF_NOCONFIRMATION;
				if ( FOF_WANTMAPPINGHANDLE == true )
					BitFlag |= (ushort)ConfigFlag.FOF_WANTMAPPINGHANDLE;
				if ( FOF_ALLOWUNDO == true )
					BitFlag |= (ushort)ConfigFlag.FOF_ALLOWUNDO;
				if ( FOF_FILESONLY == true )
					BitFlag |= (ushort)ConfigFlag.FOF_FILESONLY;
				if ( FOF_SIMPLEPROGRESS == true )
					BitFlag |= (ushort)ConfigFlag.FOF_SIMPLEPROGRESS;
				if ( FOF_NOCONFIRMMKDIR == true )
					BitFlag |= (ushort)ConfigFlag.FOF_NOCONFIRMMKDIR;
				if ( FOF_NOERRORUI == true )
					BitFlag |= (ushort)ConfigFlag.FOF_NOERRORUI;
				if ( FOF_NOCOPYSECURITYATTRIBS == true )
					BitFlag |= (ushort)ConfigFlag.FOF_NOCOPYSECURITYATTRIBS;
				if ( FOF_NORECURSION == true )
					BitFlag |= (ushort)ConfigFlag.FOF_NORECURSION;
				if ( FOF_NO_CONNECTED_ELEMENTS == true )
					BitFlag |= (ushort)ConfigFlag.FOF_NO_CONNECTED_ELEMENTS;
				if ( FOF_WANTNUKEWARNING == true )
					BitFlag |= (ushort)ConfigFlag.FOF_WANTNUKEWARNING;
				if ( FOF_NORECURSEREPARSE == true )
					BitFlag |= (ushort)ConfigFlag.FOF_NORECURSEREPARSE;
				return BitFlag;
			}

		}

	}
}

using System;

namespace WinApiUtil
{
	/// <summary>
	/// SHFileOperationの処理クラス
	/// </summary>
	public partial class SHFileOperator
	{
		#region Fields
		/// <summary>
		/// SHFileOperation関数へ渡される構造体のインスタンス
		/// ushortのビットフラグを公開したくないので、インスタンスはprivateにして各種アクセサを用意します。
		/// </summary>
		private OperationInfo _ope_info = new OperationInfo();

		/// <summary>
		/// SHFileOperationの詳細設定フラグをまとめたクラスインスタンス
		/// 中身はすべて公開されているので、そのまま返す
		/// </summary>
		public OperationDetail _ope_detail { get; } = new OperationDetail();

		/// <summary>
		/// ウィンドウハンドル
		/// </summary>
		public IntPtr WindowHandle { set { _ope_info.hwnd = value; } }

		/// <summary>
		/// 処理モード
		/// </summary>
		public Operations Mode { set { _ope_info.wFunc = value; } }

		/// <summary>
		/// 処理From
		/// 末尾の"\0\0"を自動付与する。
		/// フルパスでないファイル名が指定された場合、カレントフォルダと見做されます。
		/// </summary>
		public string pFrom { set { _ope_info.pFrom = value + '\0' + '\0'; } }

		/// <summary>
		/// 処理To
		/// 末尾の"\0\0"を自動付与する。
		/// </summary>
		public string pTo { set { _ope_info.pTo = value + '\0' + '\0'; } }

		/// <summary>
		/// 指定したファイル操作が完了する前にユーザーによって中止されたことを示します。
		/// </summary>
		public bool IsAborted { get { return _ope_info.fAnyOperationsAborted; } }

		/// <summary>
		/// 移動・コピー・名前変更されたファイルの古いファイル名と新しいファイル名を含むファイル名マッピングオブジェクトのハンドルを返します。
		/// このメンバは fFlags メンバに FOF_WANTMAPPINGHANDLE フラグが指定された場合にのみ使用されます。
		/// このハンドルが不要になったら SHFreeNameMappings 関数で解放しなくてはなりません。
		/// </summary>
		public IntPtr NameMappingsHandle { get { return _ope_info.hNameMappings; } }

		/// <summary>
		/// タイトルバーの表示する文言。
		/// 末尾の"\0"を自動付与する。
		/// </summary>
		public string TitleBarName { set { _ope_info.lpszProgressTitle = value + '\0'; } }
		#endregion

		/// <summary>
		/// コンストラクタ
		/// 先行して、処理モードのみを設定できます。
		/// </summary>
		/// <param name="mode">操作内容</param>
		public SHFileOperator( Operations mode )
		{
			_ope_info.hwnd = IntPtr.Zero;
			_ope_info.wFunc = mode;
			_ope_info.pFrom = "";
			_ope_info.pTo = "";
			_ope_info.fAnyOperationsAborted = false;
			_ope_info.hNameMappings = IntPtr.Zero;
			_ope_info.lpszProgressTitle = "";
		}

		/// <summary>
		/// 実行関数
		/// </summary>
		/// <returns></returns>
		public int Execute()
		{
			_ope_info.fFlags = _ope_detail.FlagsToBit();
			return NativeMethod.SHFileOperation( ref _ope_info );
		}
	}
}

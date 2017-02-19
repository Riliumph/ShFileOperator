using System;
using System.Collections.Generic;

namespace Extender
{
	/// <summary>
	/// String系の拡張メソッド群
	/// </summary>
	public static class StringExtender
	{
		/// <summary>
		/// 負値も取ることができるSubstring拡張メソッド
		/// PythonのSubstring的なやつを参考にしてます。
		/// </summary>
		/// <param name="instance">切り取り対象</param>
		/// <param name="begin">開始位置※負の場合、末尾から</param>
		/// <param name="end">終了位置※負の場合、末尾から</param>
		/// <returns></returns>
		public static string Substr( this string instance, int begin, int end )
		{
			int bgn_idx = begin < 0 ? instance.Length + begin : begin;
			int end_idx = end < 0 ? instance.Length + end : end;
			return instance.Substring( bgn_idx, end_idx );
		}

		/// <summary>
		/// Join拡張メソッド。もうPythonのアレです。
		/// 標準のJoinメソッドをオーバーロードしてるので、気を付けてください。
		/// </summary>
		/// <example>
		/// <code>
		/// var list = new List<string>(){"aaa","bbb","ccc"};
		/// ",".Join( list ) => aaa,bbb,ccc 
		/// </code>
		/// </example>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">連結子</param>
		/// <param name="enumerable">連結されるインスタンス</param>
		/// <returns></returns>
		public static string Join<T>( this string instance, IEnumerable<T> enumerable )
		{
			return string.Join( instance, enumerable );
		}

		public static string JoinMonad<T>( this string instance, IEnumerable<T> enumerable )
		{
			try {
				return string.Join( instance, enumerable );
			} catch ( Exception ) {
				return default( string );
			}
		}
	}
}

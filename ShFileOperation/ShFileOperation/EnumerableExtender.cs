using System;
using System.Collections.Generic;
using System.Linq;

namespace Extender
{
	/// <summary>
	/// IEnumerableの拡張メソッド群
	/// </summary>
	public static class EnumerableExtender
	{
		public static T Find<T>( this IEnumerable<T> instance, Func<T, bool> predicate )
		{
			foreach ( var current in instance ) {
				if ( predicate( current ) ) {
					return current;
				}
			}
			return default( T );
		}

		/// <summary>
		/// IEnumerableが空もしくはnullである状態を検知します。
		/// </summary>
		/// <typeparam name="T">IEnumerable type</typeparam>
		/// <param name="instance">チェック対象</param>
		/// <returns>
		///		<c>true</c> チェック対象がnull / empty
		///		<c>false</c>チェック対象がそれ以外
		/// </returns>
		public static bool IsNullOrEmpty<T>( this IEnumerable<T> instance )
		{
			if ( instance == null ) {
				return true;
			}
			// IEnumerable.Count() => O(n) / ICollection.Count =>O(1)
			var collection = instance as ICollection<T>;
			if ( collection != null ) {
				return collection.Count < 1;
			}
			return !instance.Any();
		}
	}
}

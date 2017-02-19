using System.Collections.Generic;

namespace ShFileOperation
{
	using Extender;
	using WinApiUtil;
	class Program
	{
		static void Main( string [] args )
		{
			var copy_list = new List<string> { @"c:\aaa.txt", @"c:\bbb.txt", };

			var copy_operation = new SHFileOperator( SHFileOperator.Operations.COPY );
			copy_operation.pFrom = ",".Join( copy_list );


		}
	}
}

using System.Collections.Generic;

namespace ShFileOperation
{
  using Extender;
  using System;
  using WinApiUtil;

  class Program
  {
    static void Main( string [] args )
    {
      var copy_list = new List<string> { @"c:\aaa.txt", @"c:\bbb.txt", };

      var copy_operation = new SHFileOperator( SHFileOperator.Operations.COPY );
      copy_operation.pFrom = ",".Join( copy_list );
      copy_operation._ope_detail.FOF_MULTIDESTFILES = true;
      copy_operation._ope_detail.FOF_NOCONFIRMMKDIR = true;
      // copy_operation.WindowHandle = this.Handle; // Window Form aplication code
      var result_code = copy_operation.Execute();
      switch ( result_code ) {
        // https://msdn.microsoft.com/ja-jp/library/windows/desktop/bb762164(v=vs.85).aspx
        case 0x71:  // DE_SAMEFILE
          Console.WriteLine( "The source and destination files are the same file." );
          break;
        case 0x72:  // DE_MANYSRC1DEST
          Console.WriteLine( "Multiple file paths were specified in the source buffer, but only one destination file path." );
          break;
        case 0x73:  // DE_DIFFDIR
          Console.WriteLine( "Rename operation was specified but the destination path is a different directory. Use the move operation instead." );
          break;
        case 0x74:  // DE_ROOTDIR
          Console.WriteLine( "The source is a root directory, which cannot be moved or renamed." );
          break;
        case 0x75:  // DE_OPCANCELLED
          Console.WriteLine( "The operation was canceled by the user, or silently canceled if the appropriate flags were supplied to SHFileOperation." );
          break;
        case 0x76:  // DE_DESTSUBTREE
          Console.WriteLine( "The destination is a subtree of the source." );
          break;
        case 0x78:  // DE_ACCESSDENIEDSRC
          Console.WriteLine( "Security settings denied access to the source." );
          break;
        case 0x79:  // DE_PATHTOODEEP
          Console.WriteLine( "The source or destination path exceeded or would exceed MAX_PATH." );
          break;
        case 0x7A:  // DE_MANYDEST
          Console.WriteLine( "The operation involved multiple destination paths, which can fail in the case of a move operation." );
          break;
        case 0x7C:  // DE_INVALIDFILES
          Console.WriteLine( "The path in the source or destination or both was invalid." );
          break;
        case 0x7D:  // DE_DESTSAMETREE
          Console.WriteLine( "The source and destination have the same parent folder." );
          break;
        case 0x7E:  // DE_FLDDESTISFILE
          Console.WriteLine( "The destination path is an existing file." );
          break;
        case 0x80:  // DE_FILEDESTISFLD
          Console.WriteLine( "The destination path is an existing folder." );
          break;
        case 0x81:  // DE_FILENAMETOOLONG
          Console.WriteLine( "The name of the file exceeds MAX_PATH." );
          break;
        case 0x82:  // DE_DEST_IS_CDROM
          Console.WriteLine( "The destination is a read-only CD-ROM, possibly unformatted." );
          break;
        case 0x83:  // DE_DEST_IS_DVD
          Console.WriteLine( "The destination is a read-only DVD, possibly unformatted." );
          break;
        case 0x84:  // DE_DEST_IS_CDRECORD
          Console.WriteLine( "The destination is a writable CD-ROM, possibly unformatted." );
          break;
        case 0x85:  // DE_FILE_TOO_LARGE
          Console.WriteLine( "The file involved in the operation is too large for the destination media or file system." );
          break;
        case 0x86:  // DE_SRC_IS_CDROM
          Console.WriteLine( "The source is a read-only CD-ROM, possibly unformatted." );
          break;
        case 0x87:  // DE_SRC_IS_DVD
          Console.WriteLine( "The source is a read-only DVD, possibly unformatted." );
          break;
        case 0x88:  // DE_SRC_IS_CDRECORD
          Console.WriteLine( "The source is a writable CD-ROM, possibly unformatted." );
          break;
        case 0xB7:  // DE_ERROR_MAX
          Console.WriteLine( "MAX_PATH was exceeded during the operation." );
          break;
        case 0x402:  // DE_ERROR_UNKNOWN
          Console.WriteLine( "An unknown error occurred. This is typically due to an invalid path in the source or destination." );
          break;
        case 0x10000:  // ERRORONDEST
          Console.WriteLine( "An unspecified error occurred on the destination." );
          break;
        case 0x10074:  // DE_DESTROOTDIR
          Console.WriteLine( "Destination is a root directory and cannot be renamed." );
          break;
        default:
          Console.WriteLine( "Unknown error" );
          break;
      }
    }
  }
}

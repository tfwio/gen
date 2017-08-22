/*
 * User: xo
 * Date: 6/17/2017
 * Time: 11:33 PM
 */
using System;
using System.IO;
using System.Linq;

namespace System
{
  #if CSSHELL
using w32;
using w32.shell;

  static class ShellExtensions
  {
    static public int Get_SHICON(this string path, SHGFI flags)
    {
      var shinfo = new CsShellFileInfo();
      WinShell32.SHGetFileInfo( path, 0, ref shinfo, CsShellFileInfo.SIZE, flags );
      return shinfo.IconIndex;
    }
    static public CsShellFileInfo Get_SHFILE(this DirectoryInfo directory, SHGFI flags)
    {
      return directory.FullName.Get_SHFILE(flags);
    }
    /// See SHFileInfo.Create(...)
    static public CsShellFileInfo Get_SHFILE(this string path, SHGFI flags)
    {
      var shinfo = new CsShellFileInfo();
      WinShell32.SHGetFileInfo( path, 0, ref shinfo, CsShellFileInfo.SIZE, flags );
      return shinfo;
    }
    
    static public IntPtr Get_Shell_ImageListPointer(this string path, SHGFI flags)
    {
      var shinfo = new CsShellFileInfo();
      return WinShell32.SHGetFileInfo( path, 0, ref shinfo, CsShellFileInfo.SIZE, flags );
    }
  }
  #endif
  static class FileIsLockedExtension
  {
    // https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
    static public bool IsFileLocked(this FileInfo file)
    {
      FileStream stream = null;
      try { stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None); }
      catch (IOException) { return true; }
      finally { if (stream != null) stream.Close(); }
      return false;
    }
    // https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
    static public bool IsFileStringLocked(this string filePath)
    {
      if (!File.Exists(filePath)) throw new System.IO.FileNotFoundException(filePath);
      var file = new FileInfo(filePath);
      return file.IsFileLocked();
    }
  }
  static class FileSystemExtensions
  {
    static public bool HasFlag2(this uint enumValue, uint compare)
    {
      return (enumValue & compare) == compare;
    }
    static public bool HasFlag2(this int enumValue, int compare)
    {
      return (enumValue & compare) == compare;
    }
    
    
    static public FileAttributes GetFileAttributes(this string path) { return File.GetAttributes(path); }
    static public bool IsDirectory(this string path) { return path.GetFileAttributes().HasFlag(System.IO.FileAttributes.Directory); }
    
    /// <summary>returns null or the existing FileInfo by default.</summary><seealso cref="EnsureDirectory"/>
    static public FileInfo GetFileInfo(this string path, bool nullIfNotExist=true) { return nullIfNotExist ? (File.Exists(path) ? new FileInfo(path) : null) : new FileInfo(path); }
    
    /// <summary>returns null or the existing DirectoryInfo by default.</summary><seealso cref="EnsureDirectory"/>
    static public DirectoryInfo GetDirectoryInfo(this string path, bool nullIfNotExist=true) { return nullIfNotExist ? (Directory.Exists(path) ? new DirectoryInfo(path) : null) : new DirectoryInfo(path); }
    
    // if the file or directory exists, returns File or DirectoryInfo.
    static public FileSystemInfo GetFileSystemInfo(this string path) {
      if (path.IsDirectory() && Directory.Exists(path)) return new DirectoryInfo(path);
      else if (File.Exists(path)) return new FileInfo(path);
      return null;
    }
    /// <summary>
    /// If input path is a file, we return the Directory containing it, otherwise we simply make sure that we're returning a directory.
    /// </summary>
    static public string EnsureDirectory(this string path) { return path.IsDirectory() ? new DirectoryInfo(path).FullName : new FileInfo(path).Directory.FullName; }
  }
}




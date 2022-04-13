namespace RdxCopy.CopyManager.DTOs
{
    internal class FileCopyDTO
    {
        public FileInfo FileInfo { get; set; }
        public string Src { get; set; }
        public string Dest { get; set; }
        public bool Replace { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FilesToDBConverter.Models
{
    class File
    {
        private int fileID;
        private string fileTitle;
        private string filePath;
        private string fileExtention;
        private Category fileCategory;

        public File(int _fileID, string _fileTitle, string _filePath, string _fileExtention, Category _category)
        {
            FileID = _fileID;
            FileTitle = _fileTitle;
            FilePath = _filePath;
            FileExtention = _fileExtention;
            FileCategory = _category;
            FileCategory.AddFile(this);
        }

        public int FileID { get => fileID; set => fileID = value; }
        public string FileTitle { get => fileTitle; set => fileTitle = value; }
        public string FilePath { get => filePath; set => filePath = value; }
        public string FileExtention { get => fileExtention; set => fileExtention = value; }
        internal Category FileCategory { get => fileCategory; set => fileCategory = value; }
    }
}

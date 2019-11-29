using System;
using System.Collections.Generic;
using System.Text;

namespace FilesToDBConverter.Models
{
    class Category
    {
        private int categoryID;
        private string categoryName;
        private List<File> files = new List<File>();

        public Category(int _categoryID, string _categoryName)
        {
            CategoryID = _categoryID;
            CategoryName = _categoryName;
        }

        public int CategoryID { get => categoryID; set => categoryID = value; }
        public string CategoryName { get => categoryName; set => categoryName = value; }
        internal List<File> Files { get => files; set => files = value; }

        public void AddFile(File file)
        {
            files.Add(file);
        }

    }
}

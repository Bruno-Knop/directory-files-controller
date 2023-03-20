using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace managerFiles.Models
{
    public class ListModel
    {
        public bool CopySeparatingById { get; set; } = false;
        public bool CopySeparatingByFileName { get; set; } = false;
        public bool CopySeparatingByFileType { get; set; } = false;
        public List<CopyFilesModel> CopyFiles { get; set; } = new List<CopyFilesModel>();
    }

    public class CopyFilesModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}

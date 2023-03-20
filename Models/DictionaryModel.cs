using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace managerFiles.Models
{
    public class DictionaryModel
    {
        public string Margin { get; } = "  ";
        public string Mandatory { get; } = "It is mandatory to inform an";
        public string DirectoryNotFound { get; } = "Directory informaded not Found!!";
        public string FileNotFound { get; } = "File informaded not Found!!";
        public string PathInvalid { get; } = "Path informaded invalid!!";
    }
}

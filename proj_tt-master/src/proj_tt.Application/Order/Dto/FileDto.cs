using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Order.Dto
{
    public class FileDto
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileBytes { get; set; }
    }
}
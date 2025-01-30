using Microsoft.AspNetCore.Http;

namespace Usine_Core.others.dtos
{
    public class requestUploadMultiple
    {
        public string strData { get; set; }
        public IFormFile file { get; set; }
        public IFormFile file1 { get; set; }
        public IFormFile file2 { get; set; }
        public IFormFile file3 { get; set; }
        public IFormFile file4 { get; set; }
        public IFormFile file5 { get; set; }
    }
}

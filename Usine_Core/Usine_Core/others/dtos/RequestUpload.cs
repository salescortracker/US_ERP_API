using Microsoft.AspNetCore.Http;

namespace Usine_Core.others.dtos
{
    public class RequestUpload
    {
        public string strData { get; set; }
        public IFormFile file { get; set; }       
    }
}

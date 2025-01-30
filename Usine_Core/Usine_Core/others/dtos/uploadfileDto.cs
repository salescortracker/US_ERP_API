using System;

namespace Usine_Core.others.dtos
{
    public class uploadfileDto
    {
        public string name { get; set; }
        public string size { get; set; }
        public string lastModified { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public string type { get; set; }
        public string webkitRelativePath { get; set; }
    }
}

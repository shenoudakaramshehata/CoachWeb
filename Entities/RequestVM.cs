using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Entities
{
    public class RequestVM
    {
        public List<IFormFile> File { get; set; }
        public string Json { get; set; }
    }
}

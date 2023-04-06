using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetEFAndJWT.classes
{
    public class StudentDto
    {
        public string name { get; set; } = string.Empty;
        public string major { get; set; } = string.Empty;
        public int score { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace dotnetEFAndJWT.classes
{
    public class Student
    {
        [Key]
        public int student_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string major { get; set; } = string.Empty;
        public int score { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OptionBind
{
    public class ClassTest
    {
        public string ClassName { get; set; }
        public int No { get; set; }
        public Student[] Students { get; set; }
    }

    public class Student
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flatmapper.NestedClasses
{
    public class PersonFlat
    {
        //Person properties
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        
        //Address properties
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
    }
}

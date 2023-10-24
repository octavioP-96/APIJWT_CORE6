using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.HomeModels {
    public class Person {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public int getYearsOld() {
            return DateTime.Now.Year - BirthDate.Year;
        }
    }
}

using Models.HomeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.HomeServices {
    public interface IHomeService {
        public void SetPerson(Person persona);
        public string Saludar();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Helpers {
    public class Respuesta {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public object Data { get; set; } = null!;
    }
}

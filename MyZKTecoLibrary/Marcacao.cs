using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyZKTecoLibrary
{
  public  class Marcacao
    {
        public int UserId { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
        public int Hora { get; set; }
        public int Segundo { get; set; }
        public int Minuto { get; set; }
        public DateTime DataCompleta { get; set; }
        public DateTime HorarioCompleto { get; set; }
      
    }
}

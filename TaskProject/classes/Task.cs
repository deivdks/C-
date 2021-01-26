using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.classes
{
    public class Task
    {
        public DateTime Data { get;}
        public String Titulo { get; }
        public double Hora { get; }
        public String Descricao { get; }

        public Task(DateTime Data, String Titulo, double Hora, String Descricao)
        {
            this.Data = Data;
            this.Titulo=Titulo;
            this.Hora=Hora;
            this.Descricao = Descricao;
        }


    }
}

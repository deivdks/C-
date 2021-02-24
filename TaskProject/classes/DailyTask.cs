using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.Classes
{
    public class DailyTask
    {
        public int Id { get; }
        public int IdTask { get; }
        public DateTime Data { get;}
        public string Titulo { get; }
        public double Hora { get; }
        public string Descricao { get; }

        public DailyTask(int id, int idTask, DateTime Data, string Titulo, double Hora, string Descricao)
        {
            this.Id = id;
            this.IdTask = idTask;
            this.Data = Data;
            this.Titulo=Titulo;
            this.Hora=Hora;
            //this.Descricao = Descricao;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TaskProject.Classes
{
    public static class SqlGenerate
    {

        public static String SelecttDataTask(String Data)
        {
            return String.Format("SELECT id FROM DataTask WHERE Data='{0}'", Data);
        }
        public static String InsertDataTask()
        {
            return "INSERT INTO DataTask (Data) VALUES(@data); SELECT last_insert_rowid()";
        }

        public static String InsertDescriptionTask()
        {
            return String.Format("INSERT INTO DescriptionTask (Titulo, Descricao, Hora, dataTaskId) VALUES(@titulo, @descricao, @hora, @idTask)");
        }

        public static String UpdatetDescriptionTask()
        {
            return String.Format("UPDATE DescriptionTask  SET Titulo = @titulo, Descricao = @descricao, Hora=@hora WHERE Id = @id AND DataTaskId = @idTask");
        }
        public static String SelectTasks(int idTask)
        {
            return String.Format("SELECT REPLACE(DT.Data, '/', '-') AS Data, DT.Id AS IdTask, Desc.id, Desc.Titulo, Desc.Descricao, Desc.Hora FROM DataTask DT INNER JOIN DescriptionTask Desc ON DT.Id = Desc.DataTaskId AND DT.Id={0}",idTask);
        }

        public static String DeleteDescription()
        {
            return String.Format("DELETE FROM DescriptionTask WHERE id=@idDesc AND DataTaskId=@idTask");
        }
        public static String DeleteTask()
        {
            return String.Format("DELETE FROM DataTask WHERE id=@idTask");
        }
        public static String GetLastIdDataTask()
        {
            return "SELECT MAX(Id) FROM DataTask";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TaskProject.classes
{
    public static class SqlGenerate
    {

        public static String SelecttDataTask(String Data)
        {
            return String.Format("SELECT id FROM DataTask WHERE Data='{0}'", Data);
        }
        public static String InsertDataTask(String Data)
        {
            return String.Format("INSERT INTO DataTask (Data) VALUES('{0}')",Data);
        }

        public static String InsertDescriptionTask(Task task, int IdDataTask)
        {
            String Hora = task.Hora.ToString();
            return String.Format("INSERT INTO DescriptionTask (Titulo, Descricao, Hora, dataTaskId) VALUES('{0}','{1}','{2}','{3}')", task.Titulo , task.Descricao, Hora.Replace(",",".") , IdDataTask);
        }

        public static String UpdatetDescriptionTask(Task task, int IdDescriptionTask , int IdDataTask)
        {
            String Hora = task.Hora.ToString(); /* Meio encontrado para montar o valor com ponto ao inves de virgula no sql*/
            return String.Format("UPDATE DescriptionTask  SET Titulo = '{0}', Descricao = '{1}', Hora={2} WHERE Id = {3} AND DataTaskId = {4}", task.Titulo, task.Descricao, Hora.Replace(",", "."), IdDescriptionTask, IdDataTask);
        }
        public static String SelectTasks(int idTask)
        {
            return String.Format("SELECT REPLACE(DT.Data, '/', '-') AS Data, DT.Id AS IdTask, Desc.id, Desc.Titulo, Desc.Descricao, Desc.Hora FROM DataTask DT INNER JOIN DescriptionTask Desc ON DT.Id = Desc.DataTaskId AND DT.Id={0}",idTask);
        }

        public static String DeleteDescription(int IdDescriptionTask, int IdDataTask)
        {
            return String.Format("DELETE FROM DescriptionTask WHERE id={0} AND DataTaskId={1}",IdDescriptionTask, IdDataTask);
        }
        public static String DeleteTask(int IdDataTask)
        {
            return String.Format("DELETE FROM DataTask WHERE id={0} ", IdDataTask);
        }
        public static String GetLastIdDataTask()
        {
            return "SELECT MAX(Id) FROM DataTask";
        }
    }
}

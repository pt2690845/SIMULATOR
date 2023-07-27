using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMULATOR
{
    public class MultiThreadOperation
    {
        public static Task thread = new Task(executeTaskFunctions);
        public static Action? threadFunctions;
        public static bool doTask;
        public static void executeTaskFunctions()
        {
            if (threadFunctions == null) throw new Exception("No se han declarado tareas que ejecutar en segundo plano");
            while (true)
            {
                if (doTask)
                {
                    threadFunctions();
                }
            }
        }
    }
}

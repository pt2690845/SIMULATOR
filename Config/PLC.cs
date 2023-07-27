using SIMULATOR.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace SIMULATOR
{
    public static class Config
    {
        public static bool defaultValue = false;

        private static string VariablesDirection = @"C:\Users\hecto\source\repos\SIMULATOR\SIMULATOR\Config\Variables.csv";

        public static void ReadVariables()
        {
            StreamReader sr = new StreamReader(VariablesDirection);
            if (sr == null) throw new Exception("Error de lectura del fichero: " + VariablesDirection);
            sr.ReadLine(); // Ignoramos la línea de títulos
            List<Variable> variables = new List<Variable> { };
            while (!sr.EndOfStream)
            {
                string ?line = sr.ReadLine();       
                if (line == null) throw new Exception("Error de lectura del fichero: " + VariablesDirection);
                string[] values = line.Split(';');
                switch (values[2])
                {
                    case "1_bit":
                        variables.Add(new BitVariable(values[0], values[1], values[2], values[3], values[4], values[5], defaultValue, values[6]));
                        break;
                    case "8_bit_unsigned":
                        variables.Add(new ByteVariable(values[0], values[1], values[2], values[3], values[4], values[5], defaultValue, values[6]));
                        break;
                    case "32_bit_signed":
                        variables.Add(new IntVariable(values[0], values[1], values[2], values[3], values[4], values[5], defaultValue, values[6]));
                        break;
                    case "32_bit_float":
                        variables.Add(new IntVariable(values[0], values[1], values[2], values[3], values[4], values[5], defaultValue, values[6]));
                        break;
                    default:
                        throw new Exception("Tipo de variable incorrecto o no soportado");
                }
            }
            PLCMemory.AllVariables = variables;
        }
    }
}

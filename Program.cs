using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.App;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {   //AppDomain.CurrentDomain.ProcessExit += método que se va a ejecutar
            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");
            //Printer.Beep(10000, cantidad: 10);
            ImpimirCursosEscuela(engine.Escuela);
            // Si no se quieren pasar todos los parámetros de salida se puede crear una variable dummy para que se validen
            var listaObjetos = engine.GetObjetosEscuela(out int conteoEvaluaciones,
                                                        out int conteoCursos,
                                                        out int conteoAsignaturas,
                                                        out int conteoAlumnos);
            var dicc = engine.GetDiccionarioObjetos();  
            engine.ImprimirDiccionario(dicc,true);  
            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
            var evalList = reporteador.GetListaEvaluaciones();
            var listAsig = reporteador.GetListaAsignaturas();
            var listEvalxAsig = reporteador.GetDicEvaluacionAsignatura();
            
            #region Comentado para estudio Linq      
            // engine.Escuela.LimpiarLugar();
            // var ListaILugar = from obj in listaObjetos
            //                   where obj is ILugar
            //                   select (ILugar) obj;
            #endregion

        }

        private static void ImpimirCursosEscuela(Escuela escuela)
        {

            Printer.WriteTitle("Cursos de la Escuela");


            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre  }, Id  {curso.UniqueId}");
                }
            }
        }
    }
}

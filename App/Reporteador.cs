using System.Collections.Generic;
using CoreEscuela.Entidades;
using System.Linq;
using System;

namespace CoreEscuela.App
{
    public class Reporteador
    {   
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if (dicObsEsc == null)
            {
                throw new ArgumentException(nameof(dicObsEsc));
            }
            _diccionario = dicObsEsc;
        }

        public IEnumerable<Escuela> GetEscuela()
        {
            IEnumerable<Escuela> rta;
           if (_diccionario.TryGetValue(LlaveDiccionario.Escuela, 
                                        out IEnumerable<ObjetoEscuelaBase> lista))
           {
               rta = lista.Cast<Escuela>();
           }
           else
           {
               rta = null; 
           }
            
            return rta;
        }
        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
    
           if (_diccionario.TryGetValue(LlaveDiccionario.Evaluacion, 
                                        out IEnumerable<ObjetoEscuelaBase> lista))
           {
               return lista.Cast<Evaluación>();
           }
           else
           {
               return new List<Evaluación>(); 
           }
            
           
        }
        // Sobre carga para cuando no quiera que me devuelva la lista de asignaturas
        public IEnumerable<string> GetListaAsignaturas() 
        {
           return GetListaAsignaturas(out var dummy);
        }

        public IEnumerable<string> GetListaAsignaturas(
        // para devolver la lista de evaluaciones y luego poder usarla en el query de Eva x Asig
                                    out IEnumerable<Evaluación> listaEvaluaciones) 
        {
            listaEvaluaciones = GetListaEvaluaciones();

            return (from ev in listaEvaluaciones
                    // where ev.Nota >= 3.0f
                    select ev.Asignatura.Nombre).Distinct(); ;
           
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetDicEvaluacionAsignatura()
        {
            var dicRta = new Dictionary<string, IEnumerable<Evaluación>>();
            var listaAsig = GetListaAsignaturas(out var listEva);
            
            foreach (var asig in listaAsig)
            {
                var evalAsig = from eval in listEva
                                where eval.Asignatura.Nombre == asig
                                select eval;
                
                dicRta.Add(asig, evalAsig);
            }

            return dicRta;
        }

        public Dictionary<string, IEnumerable<object>> GetPromedioAlumnosPorAsignatura()
        {
            var rta = new Dictionary<string, IEnumerable<object>>();
            var DicEvaAsig = GetDicEvaluacionAsignatura();

            foreach (var AsiEval in DicEvaAsig)
            {
                var dummy = from eval in AsiEval.Value
                            group eval by eval.Alumno.UniqueId
                            into grupoEvalAlumno
                            select new 
                            {
                                AlumnoId = grupoEvalAlumno.Key,
                                Promedio = grupoEvalAlumno.Average(evaluacion => evaluacion.Nota)
                            };
            }


            return rta;
        }
    }
}
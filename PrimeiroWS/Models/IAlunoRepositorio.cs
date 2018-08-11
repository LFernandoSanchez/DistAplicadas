using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeiroWS.Models
{
    public interface IAlunoRepositorio
    {

        //Definição dos métodos a serem implementados
        IEnumerable<Aluno> GetAll();
        IEnumerable<Aluno> GetPorCurso(string curso);
        Aluno GetPorId(int id);
        Aluno Add(Aluno item);
        void Remove(int id);
        void Update(Aluno item);
    }
}

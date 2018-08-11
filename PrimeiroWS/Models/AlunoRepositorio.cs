using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrimeiroWS.Models;

namespace PrimeiroWS.Models
{
    public class AlunoRepositorio : IAlunoRepositorio
    {

        private List<Aluno> alunos;
        private int _nextId;

        public AlunoRepositorio()
        {
            alunos = new List<Aluno>();
            _nextId = 1;

            Add(new Aluno { Nome = "João Paulo", Curso = "Mecatrônica" });
            Add(new Aluno { Nome = "Caio", Curso = "Informática" });
            Add(new Aluno { Nome = "Maria Clara", Curso = "Eletro" });
            Add(new Aluno { Nome = "Eduardo", Curso = "Enfermagem" });
            
        }

        //Retorna tudo da lista de alunos
        public IEnumerable<Aluno> GetAll()
        {
            return alunos;
        }

        public IEnumerable<Aluno> GetPorCurso(string curso) {
            return null;
        }
        public Aluno GetPorId(int id) {
             return alunos.Find(alu => alu.Id == id); ;
        }
        public Aluno Add(Aluno item) {
            if (item == null)
            {
                throw new ArgumentNullException("item nulo");
            }
            item.Id = _nextId++;
            alunos.Add(item);
            return item;
        }
        public void Remove(int id) {
            alunos.RemoveAll(a => a.Id == id);
        }
        public void Update(Aluno item) {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            alunos.Where(a => a.Id == item.Id)
            .Select(a =>
            {
                a.Id = item.Id;
                a.Nome = item.Nome;
                a.Curso = item.Curso;
                return a;
            }).ToList();
        }


    }
}
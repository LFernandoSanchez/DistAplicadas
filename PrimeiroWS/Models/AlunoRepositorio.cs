using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
        }

        //Retorna tudo da lista de alunos
        public IEnumerable<Aluno> GetAll()
        {
            string strConexao = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(strConexao);
            String sql = "SELECT * FROM ALUNOS";
            SqlCommand comando = new SqlCommand(sql);

            comando.Connection = sqlConnection;

            sqlConnection.Open();

            SqlDataReader leitor = comando.ExecuteReader();

            while (leitor.Read())
            {
                Aluno al = new Aluno();
                al.Id = leitor.GetInt32(0);
                al.Nome = leitor.GetString(1);
                al.Curso = leitor.GetString(2);

                alunos.Add(al);
            }

            leitor.Close();
            sqlConnection.Close();

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
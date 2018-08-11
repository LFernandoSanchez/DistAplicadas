using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PrimeiroWS.Models;


namespace PrimeiroWS.Controllers
{
    [RoutePrefix("api/alunos")]
    public class AlunosController : ApiController
    {

        static readonly IAlunoRepositorio repositorio = new AlunoRepositorio();
        /* Método: GetAllAlunos(): IEnumerable<Aluno>
        *
        * Consulta todos os alunos, chamando o método GetAll(): IEnumerable<Aluno>
        * da classe AlunoRepositorio.cs
        *
        */
        public IEnumerable<Aluno> GetAllAlunos()
        {
            return repositorio.GetAll();
        }
        /* Método: GetAlunoPorCurso(string curso): IEnumerable<Aluno>
        *
        * Consulta todos os alunos, chamando o método GetPorCurso(string curso): Aluno
        * da classe AlunoRepositorio.cs
        *
        */
        public IEnumerable<Aluno> GetAlunoPorCurso(string curso)
        {
            // MessageBox.Show("Controller");
            // return repositorio.GetAll().Where(
            // a => string.Equals(a.Curso, curso, StringComparison.OrdinalIgnoreCase));
            return repositorio.GetPorCurso(curso).Where(
            a => string.Equals(a.Curso, curso, StringComparison.OrdinalIgnoreCase));
        }
        /* Método: GetAlunoPorId(int id): Aluno
        *
        * Consulta todos os alunos, chamando o método GetPorId(int id): Aluno
        * da classe AlunoRepositorio.cs
        *
        */
        public Aluno GetAlunoPorId(int id)
        {
            //O Objeto item recebe o retorno do método Get do repositório
            Aluno item = repositorio.GetPorId(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            //retorna no formato json
            return item;
        }
        //Inclui um novo aluno
        //HttpResponseMessage: mensagem de resposta que será trafegada dentro do protocolo HTTP
        public HttpResponseMessage PostAluno(Aluno item)
        {
            item = repositorio.Add(item);
            // O objeto Request está dentro da classe APIController
            // Coloque o cursor sobre Request e digite F12
            //Uma das assinaturas do CreateResponse pede para passar o status HTTP do response
            var response =
            Request.CreateResponse<Aluno>(HttpStatusCode.Created, item);
            //A URL pede o Id do aluno
            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
        //Altera
        public HttpResponseMessage PutAluno(Aluno item)
        {
            Aluno aluno = repositorio.GetPorId(item.Id);
            if (aluno == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            repositorio.Update(item);
            var response = Request.CreateResponse<Aluno>(HttpStatusCode.Created, item);
            return response;
        }
        //Exclui
        public void DeleteAluno(int id)
        {
            Aluno item = repositorio.GetPorId(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            repositorio.Remove(id);
        }
    }
}

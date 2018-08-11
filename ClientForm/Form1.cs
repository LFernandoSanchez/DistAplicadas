using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Form1 : Form
    {
        private string URI = "";
        private int nextId = 1;
        private string mensagemURI = "Preencha o campo porta da URI";
        Aluno aluno = new Aluno();
        HttpClient client = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();
        public Form1()
        {
            InitializeComponent();
        }

        private async void GetAll()
        {
            URI = tbURI.Text;
            if (!preencherURI(URI))
                MessageBox.Show(mensagemURI);
            else
            {
                /*
                * Método HttpClient.GetAsync (Uri)
                *
                * Envia uma requisição GET para o URI especificado
                * com uma operação assíncrona.
                */
                //O objeto response(HttpResponseMessage) recebe a resposta do envio de requisição ao endereço URI
                response = await client.GetAsync(URI);
                //Se o envio de requisição fdor atendido
                if (response.IsSuccessStatusCode)
                {
                    /*
                    * Propriedade HttpResponseMessage.Content
                    * Obtém ou define o conteúdo de uma mensagem de resposta HTTP.
                    *
                    * Para criar código assíncrono usamos as palavras chaves async
                    * e await onde por padrão um método modificado por uma palavra-chave
                    * async contém pelo menos uma expressão await.
                    */
                    //A variável AlunoJsonString recebe o resultado da requisição
                    var AlunoJsonString = await response.Content.ReadAsStringAsync();
                    /*
                    * Classe JsonConvert
                    *
                    * Fornece métodos para conversão entre tipos .NET e tipos JSON.
                    *
                    * DeserializeObject<T>(String)
                    *
                    */
                    gvAluno.DataSource = JsonConvert.DeserializeObject<Aluno[]>(AlunoJsonString).ToList();
                }
                else
                    MessageBox.Show("Não foi possível obter o produto : " + response.StatusCode);
            }
        }
        private async void GetAlunoByCurso(string nomeCurso)
        {
            URI = tbURI.Text;
            if (!preencherURI(URI))
                MessageBox.Show(mensagemURI);
            else
            {
                //O BindSource é usado para vincular dados de um objeto a um um componente do windows form (GridView)
                BindingSource bsDados = new BindingSource();
                URI = tbURI.Text + "/?curso=" + nomeCurso.ToString();
                response = await client.GetAsync(URI);
                if (response.IsSuccessStatusCode)
                {
                    var AlunoJsonString = await response.Content.ReadAsStringAsync();
                    bsDados.DataSource = JsonConvert.DeserializeObject<Aluno[]>(AlunoJsonString).ToList();
                    gvAluno.DataSource = bsDados;
                }
                else
                {
                    MessageBox.Show("Falha ao obter o aluno pelo curso : " + response.StatusCode);
                }
            }
        }
        private async void GetAlunoById(int id)
        {
            URI = tbURI.Text;
            if (!preencherURI(URI))
                MessageBox.Show(mensagemURI);
            else
            {
                BindingSource bsDados = new BindingSource();
                URI = tbURI.Text + "/" + id.ToString();
                response = await client.GetAsync(URI);
                if (response.IsSuccessStatusCode)
                {
                    var AlunoJsonString = await response.Content.ReadAsStringAsync();
                    bsDados.DataSource = JsonConvert.DeserializeObject<Aluno>(AlunoJsonString);
                    gvAluno.DataSource = bsDados;
                }
                else
                {
                    MessageBox.Show("Falha ao obter o aluno : " + response.StatusCode);
                }
            }
        }
        private async void AddAluno()
        {
            URI = tbURI.Text;
            aluno.Id = nextId++;
            aluno.Nome = tbNome.Text;
            aluno.Curso = tbCurso.Text;
            if (!preencherURI(URI))
                MessageBox.Show(mensagemURI);
            else
            {
                var serializedAluno = JsonConvert.SerializeObject(aluno);
                //A classe StringContent adiciona o conteúdo json em um objeto HTTP
                var content = new StringContent(serializedAluno, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(URI, content);
            }
            GetAll();
        }
        private async void UpdateAluno(int id)
        {
            URI = tbURI.Text;
            aluno.Id = id;
            aluno.Nome = tbNome.Text;
            aluno.Curso = tbCurso.Text;
            if (!preencherURI(URI))
                MessageBox.Show(mensagemURI);
            else
            {
                response = await client.PutAsJsonAsync(URI, aluno);
                if (response.IsSuccessStatusCode)
                    MessageBox.Show("Aluno atualizado");
                else
                    MessageBox.Show("Falha ao atualizar o aluno : " + response.StatusCode);
            }
            GetAll();
        }
        private async void DeleteAluno(int id)
        {
            URI = tbURI.Text;
            int alunoID = id;
            if (!preencherURI(URI))
                MessageBox.Show(mensagemURI);
            else
            {
                URI = tbURI.Text + "/" + alunoID;
                response = await client.DeleteAsync(URI);
                if (response.IsSuccessStatusCode)
                    MessageBox.Show("Aluno excluído com sucesso");
                else
                    MessageBox.Show("Falha ao excluir o aluno : " + response.StatusCode);
            }
            GetAll();
        }
        public bool preencherURI(string uri)
        {
            if (uri == "http://localhost:porta/api/alunos")
                return false;
            else
                return true;
        }
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            GetAll();
        }
        private void btnConsultarCurso_Click(object sender, EventArgs e)
        {
            aluno.Curso = tbCurso.Text;
            if (aluno.Curso != "")
                GetAlunoByCurso(aluno.Curso);
        }
        private void btnInserir_Click(object sender, EventArgs e)
        {
            AddAluno();
        }
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (tbId.Text == "")
                MessageBox.Show("Preencha o campo Id");
            else
            {
                aluno.Id = Convert.ToInt32(tbId.Text);
                if (aluno.Id != -1)
                    UpdateAluno(aluno.Id);
            }
        }
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (tbId.Text == "")
                MessageBox.Show("Preencha o campo Id");
            else
            {
                aluno.Id = Convert.ToInt32(tbId.Text);
                if (aluno.Id != -1)
                    DeleteAluno(aluno.Id);
            }
        }
        private void btnPorId_Click(object sender, EventArgs e)
        {
            if (tbId.Text == "")
                MessageBox.Show("Preencha o campo Id");
            else
            {
                aluno.Id = Convert.ToInt32(tbId.Text);
                GetAlunoById(aluno.Id);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tbURI.Text = "http://localhost:porta/api/alunos";
        }
    }
}
    


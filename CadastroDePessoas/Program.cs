using System;
using System.Net.Sockets;
namespace CadastroPessoas
{
    public class PessoaVO
    {
        public string nome { get; set; }
        public string email { get; set; }
        public DateTime dataNascimento { get; set; }
        public int idade => DateTime.Now.Year - dataNascimento.Year;
    }

    public class CadastroHelper
    {
        //validar dados
        public static bool ValidarPessoa(PessoaVO pessoa, out string erro) //retornar mais de um valor de um método / permitir que o método modifique o valor da variável passada
        {
            erro = "";

            //nome: nao pode ser vazio, menor que 2, e precisa ter nome e sobrenome
            if (string.IsNullOrWhiteSpace(pessoa.nome) || pessoa.nome.Trim().Length < 2 || !pessoa.nome.Contains(" "))
            {
                erro = "Nome inválido";
                return false;
            }

            //email: deve conter @ e pelo menos 3 caracteres antes e depois
            int caracteres = pessoa.email.IndexOf('@');
            if (caracteres < 3 || caracteres == -1 || pessoa.email.Length - caracteres - 1 < 3)
            {
                erro = "Email inválido";
                return false;
            }

            //idade: nao pode ser negativa nem maior que 150
            int idade = DateTime.Now.Year - pessoa.dataNascimento.Year;
            if (idade < 0 || idade > 150)
            {
                erro = "Idade inválida";
                return false;
            }

            return true;
        }
    }

    public class Program
    {
        static List<PessoaVO> pessoas = new List<PessoaVO>();

        public static void Main()
        {
            Console.WriteLine("Cadastro de Pessoas");
            Console.WriteLine();
            string continuar = ""; //declara variável

            do
            {
                PessoaVO p = new PessoaVO();

                Console.Write("Nome: ");
                p.nome = Console.ReadLine();

                Console.Write("Email: ");
                p.email = Console.ReadLine();

                Console.Write("Data de Nascimento (dd-MM-yyyy): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime data))
                {
                    Console.WriteLine("Data inválida!");
                    continue;
                }
                else
                {
                    p.dataNascimento = data;

                    if (CadastroHelper.ValidarPessoa(p, out string erro))
                    {
                        pessoas.Add(p);
                        Console.WriteLine("Pessoa cadastrada com sucesso.");
                    }
                    else
                    {
                        Console.WriteLine("Erro: " + erro);
                    }

                    Console.Write("Deseja cadastrar outra pessoa? (s/n): ");
                    continuar = Console.ReadLine();
                }
            }
            while (continuar.ToLower() == "s");

            SalvarRelatorio();
            MaisNovaEMaisVelha();
        }

        static void SalvarRelatorio()
        {
            using (StreamWriter writer = new StreamWriter(@"C:\arquivosATP\relatorio_pessoas.txt"))
            {
                foreach (var p in pessoas)
                {
                    writer.WriteLine($"{p.nome}; {p.email}; {p.dataNascimento: dd-MM-yyyy}; {p.idade} anos");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Relatório salvo em relatorio_pessoas.txt");
        }

        static void MaisNovaEMaisVelha()
        {
            if (pessoas.Count == 0) return;

            PessoaVO maisNova = pessoas[0];
            PessoaVO maisVelha = pessoas[0];

            for (int i = 0; i < pessoas.Count; i++)
            {
                if (pessoas[i].dataNascimento > maisNova.dataNascimento)
                    maisNova = pessoas[i];

                if (pessoas[i].dataNascimento < maisVelha.dataNascimento)
                    maisVelha = pessoas[i];
            }

            Console.WriteLine($"\nPessoa mais nova: {maisNova.nome} - {maisNova.idade} anos");
            Console.WriteLine($"Pessoa mais velha: {maisVelha.nome} - {maisVelha.idade} anos");
        }

    }
}

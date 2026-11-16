
using System.Reflection;
using GestaoJogosEFCore.Data;
using GestaoJogosEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoJogosEFCore.Repositories
{

    public class JogoRepository
    {
        private readonly DbContextOptions<GestaoJogosContext> _options;

        public JogoRepository(DbContextOptions<GestaoJogosContext> options)
        {
            _options = options;
        }

        public List<Jogo> Listar()
        {
            using var context = new GestaoJogosContext(_options);

            return context.Jogo
            .Include(j => j.IdDesenvolvedoraNavigation)
            .OrderBy(j => j.IdJogo)
            //executa a consulta no banco de dados e materializa os resultados em uma lista
            .ToList();
        }

        // Busca um único jogo por meio do id, retornando null caso nao seja encontrado
        public Jogo? BuscarPorId(int id)
        {
            using var context = new GestaoJogosContext(_options);

            return context.Jogo
                .Include(j => j.IdDesenvolvedoraNavigation)
                // retornaa o primeiro resultado ou o valor padrão (null)
                .FirstOrDefault(j => j.IdJogo == id);
        }

        public void Adicionar(Jogo jogo)
        {
            using var context = new GestaoJogosContext(_options);

            context.Jogo.Add(jogo);
            context.SaveChanges(); // funciona como o commit no SQL
        }

        public bool Atualizar(Jogo jogoAtualizado)
        // CRUD = Create, Read, Update, Delete
        {
            using var context = new GestaoJogosContext(_options);

            var jogoExistente = context.Jogo.Find(jogoAtualizado.IdJogo);
            if (jogoExistente is null)
            {
                return false;
            }
            // copia para a entidade rastreada os valores recebidos pelo parametro da função
            jogoExistente.IdDesenvolvedora = jogoAtualizado.IdDesenvolvedora;
            jogoExistente.Nome = jogoAtualizado.Nome;
            jogoExistente.Genero = jogoAtualizado.Genero;
            jogoExistente.AnoLancamento = jogoAtualizado.AnoLancamento;
            jogoExistente.Preco = jogoAtualizado.Preco;

            context.SaveChanges();
            return true;
        }

        public bool Excluir(int id)
        {
            using var context = new GestaoJogosContext(_options);

            var jogoExistente = context.Jogo.Find(id);
            if (jogoExistente is null)
            {
                return false;
            }

            context.Jogo.Remove(jogoExistente);
            context.SaveChanges();
            return true;
        }


    public List<Desenvolvedora> ListarDesenvolvedoras()
        {
            using var context = new GestaoJogosContext(_options);
            return context.Desenvolvedora
            .OrderBy(d => d.Nome)
            .ToList();
           
        }

    public void AdicionarDesenvolvedora(Desenvolvedora desenvolvedora)
        {
            using var context = new GestaoJogosContext(_options);
            context.Desenvolvedora.Add(desenvolvedora);
            context.SaveChanges();
        }

    }
}
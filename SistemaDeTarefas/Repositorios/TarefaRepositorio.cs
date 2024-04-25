using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Repositorios
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly SistemaTarefasDBContex _dbContex;

        public TarefaRepositorio(SistemaTarefasDBContex sistemaTarefasDBContex)
        {
            _dbContex = sistemaTarefasDBContex;
        }
        public async Task<TarefaModel> BuscarPorId(int id)
        {
            return await _dbContex.Tarefas.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<TarefaModel>> BuscarTodasTarefas()
        {
            return await _dbContex.Tarefas.ToListAsync();
        }
        public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
        {
            await _dbContex.Tarefas.AddAsync(tarefa);
            await _dbContex.SaveChangesAsync();

            return tarefa;
        }
        public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
        {
            TarefaModel tarefaPorId = await BuscarPorId(id);

            if (tarefaPorId == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não foi encontrado no banco de dados.");
            }

            tarefaPorId.Nome = tarefa.Nome;
            tarefaPorId.Descricao = tarefa.Descricao;
            tarefaPorId.Status = tarefa.Status;
            tarefaPorId.UsuarioId = tarefa.UsuarioId;
            

            _dbContex.Tarefas.Update(tarefaPorId);
            await _dbContex.SaveChangesAsync();

            return tarefaPorId;
        }
        public async Task<bool> Apagar(int id)
        {
            TarefaModel tarefaPorId = await BuscarPorId(id);

            if (tarefaPorId == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não foi encontrado no banco de dados.");
            }

            _dbContex.Tarefas.Remove(tarefaPorId);
            await _dbContex.SaveChangesAsync();

            return true;
        }
    }
}

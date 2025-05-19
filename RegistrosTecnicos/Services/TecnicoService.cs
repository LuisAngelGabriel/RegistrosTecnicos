using Microsoft.EntityFrameworkCore;
using RegistrosTecnicos.DAL;
using RegistrosTecnicos.Models;
using System.Linq.Expressions;

namespace RegistrosTecnicos.Services
{
    public class TecnicosService(IDbContextFactory<Contexto> DbFactory)
    {
        private readonly IDbContextFactory<Contexto> DbFactory = DbFactory;

        public async Task<bool> Guardar(Tecnico tecnico)
        {
            if (string.IsNullOrWhiteSpace(tecnico.Nombre) || tecnico.SueldoHora <= 0)
                return false;

            if (await ExisteNombre(tecnico.Nombre, tecnico.TecnicoId))
                return false;

            if (!await Existe(tecnico.TecnicoId))
                return await Insertar(tecnico);
            else
                return await Modificar(tecnico);
        }

        private async Task<bool> Existe(int tecnicoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.AnyAsync(t => t.TecnicoId == tecnicoId);
        }

        private async Task<bool> ExisteNombre(string nombre, int tecnicoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.AnyAsync(t => t.Nombre == nombre && t.TecnicoId != tecnicoId);
        }

        private async Task<bool> Insertar(Tecnico tecnico)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Tecnicos.Add(tecnico);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Tecnico tecnico)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Tecnicos.Update(tecnico);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<Tecnico?> Buscar(int tecnicoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.FirstOrDefaultAsync(t => t.TecnicoId == tecnicoId);
        }

        public async Task<bool> Eliminar(int tecnicoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos
                .Where(t => t.TecnicoId == tecnicoId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Tecnico>> Listar(Expression<Func<Tecnico, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

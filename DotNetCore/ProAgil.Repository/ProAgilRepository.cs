using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        public ProAgilContext context { get; }
        public ProAgilRepository(ProAgilContext context){
            this.context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            this.context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            this.context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            this.context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await(this.context.SaveChangesAsync())) > 0;
        }

        //EVENTO
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = this.context.Eventos
                                .Include(c => c.Lotes)
                                .Include(c => c.RedesSociais);
            if(includePalestrantes){
                query = query
                        .Include(pe=>pe.PalestranteEventos)
                        .ThenInclude(p=>p.Palestrante);
            }
            query = query.AsNoTracking().OrderByDescending(c => c.DataEvento);

            return await query.ToArrayAsync();
        }
        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = this.context.Eventos
                                .Include(c => c.Lotes)
                                .Include(c => c.RedesSociais);
            if(includePalestrantes){
                 query = query
                         .Include(pe=>pe.PalestranteEventos)
                         .ThenInclude(p=>p.Palestrante);
            }
            query = query.OrderByDescending(c => c.DataEvento)
                         .AsNoTracking()
                         .Where(c=>c.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoAsyncById(int eventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = this.context.Eventos
                                .Include(c => c.Lotes)
                                .Include(c => c.RedesSociais);
            if(includePalestrantes){
                query = query
                        .Include(pe=>pe.PalestranteEventos)
                        .ThenInclude(p=>p.Palestrante);
            }
            query = query.OrderByDescending(c => c.DataEvento)
                         .Where(c=>c.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }

        //PALESTRANTE
        public async Task<Palestrante[]> GetAllPalestranteAsyncByNome(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = this.context.Palestrantes
                                                .Include(c => c.RedesSociais);
            if(includeEventos){
                query = query
                        .Include(pe=>pe.PalestranteEventos)
                        .ThenInclude(p=>p.Evento);
            }
            query = query.AsNoTracking().Where(c=>c.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }
        public async Task<Palestrante> GetPalestranteAsyncById(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = this.context.Palestrantes
                                .Include(c => c.RedesSociais);
            if(includeEventos){
                query = query
                        .Include(pe=>pe.PalestranteEventos)
                        .ThenInclude(p=>p.Evento);
            }
            query = query.OrderBy(c => c.Nome)
                         .AsNoTracking()
                         .Where(c=>c.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }
            }
}
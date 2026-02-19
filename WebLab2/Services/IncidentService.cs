using WebLab2.DataBase;
using WebLab2.Interfaces;
using WebLab2.Models;
using Microsoft.EntityFrameworkCore;

namespace WebLab2.Services
{
    public class IncidentService: IIncidentService
    {
        private readonly AppDbContext _context;

        public IncidentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Incident>> Get10Incidents()
        {
            var incidents = await _context.Incidents.OrderByDescending(i => i.Time).Take(10).ToListAsync();
            return incidents;
        }
    }
}

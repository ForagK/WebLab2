using WebLab2.Models;

namespace WebLab2.Interfaces
{
    public interface IIncidentService
    {
        public Task<List<Incident>> Get10Incidents();
    }
}

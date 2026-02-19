using WebLab2.Models;

namespace WebLab2.Interfaces
{
    public interface IAdminService
    {
        public Task<Admin?> GetAdmin(int id);
    }
}

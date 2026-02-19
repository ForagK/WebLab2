using Azure.Core;
using Microsoft.EntityFrameworkCore;
using WebLab2.DataBase;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Services
{
    public class AdminService: IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Admin?> GetAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return null;
            return admin;
        }
    }
}

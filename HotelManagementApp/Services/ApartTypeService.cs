using HotelManagementApp.Models;
using HotelManagementApp.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementApp.Services
{
	public class ApartTypeService
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger _logger;

		public ApartTypeService(ApplicationDbContext db, ILogger<ApartTypeService> logger)
		{
			this._db = db;
			this._logger = logger;
		}

		public async Task<List<ApartTypeModel>> GetAllTypes()
		{
			return await _db.ApartTypes.ToListAsync();
		}
	}
}
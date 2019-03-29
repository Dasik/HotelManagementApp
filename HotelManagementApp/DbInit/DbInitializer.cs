using HotelManagementApp.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HotelManagementApp.Models.Context;

namespace HotelManagementApp.DbInit
{
	public class DbInitializer
	{

		public static async Task InitializeAsync(ApplicationDbContext db)
		{
			//Phone p1 = new Phone { Name = "Samsung Galaxy S5", Price = 14000 };
			//Phone p2 = new Phone { Name = "Nokia Lumia 630", Price = 8000 };

			//db.Phones.Add(p1);
			//db.Phones.Add(p2);
			if (db.Hotels.Any())
				return;
			var hotel = new HotelModel
			{
				ContactEmail = "hotel@mail.com",
				ContactPhone = "+71234567890",
				Description = "Стандартное описание отеля",
				Name = "Hotel #1",
				Image = await readImage("images\\Hotel\\Hotel1.jpg")
			};
			await db.Hotels.AddAsync(hotel);

			var apartTypes = new List<ApartTypeModel>
			{
				new ApartTypeModel()
				{
					Description = "Стандартное описание стандартного типа",
					Name = "Стандарт",
					Price = 1000
				},
				new ApartTypeModel()
				{
					Description = "Стандартное описание полулюкса",
					Name = "Полулюкс",
					Price = 2000
				},
				new ApartTypeModel()
				{
					Description = "Стандартное описание люкса",
					Name = "Люкс",
					Price = 3000
				}
			};

			await db.ApartTypes.AddRangeAsync(apartTypes);

			var apart1 = new ApartmentModel()
			{
				Type = apartTypes[0],
				Description = apartTypes[0].Description,
				Hotel = hotel,
				Image = await readImage("images\\Aparts\\Apart1.jpg"),
				Capacity = 1,
				RoomNumber = "1a"
			};
			var apart2 = new ApartmentModel()
			{
				Type = apartTypes[1],
				Description = apartTypes[1].Description,
				Hotel = hotel,
				Image = await readImage("images\\Aparts\\Apart2.jpg"),
				Capacity = 2,
				RoomNumber = "2a"
			};
			var apart3 = new ApartmentModel()
			{
				Type = apartTypes[2],
				Description = apartTypes[2].Description,
				Hotel = hotel,
				Image = null,
				Capacity = 3,
				RoomNumber = "3a"
			};
			await db.Apartments.AddRangeAsync(apart1, apart2, apart3);

			for (int i = 0; i < 10; i++)
			{
				var apartType = apartTypes[i % 3];
				await db.Apartments.AddAsync(new ApartmentModel()
				{
					Type = apartType,
					Description = apartType.Description,
					Hotel = hotel,
					Image = null,
					Capacity = i % 5 + 1,
					RoomNumber = i + "в"
				});
			}

			await db.SaveChangesAsync();
		}

		public static async Task<byte[]> readImage(string imagePath)
		{
			imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);
			using (var img = System.IO.File.OpenRead(imagePath))
			{
				var imageBytes = new byte[img.Length];
				await img.ReadAsync(imageBytes, 0, (int)img.Length);
				return imageBytes;
			}
		}
	}
}
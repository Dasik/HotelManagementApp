using HotelManagementApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HotelManagementApp.ViewModels.Db;

namespace HotelManagementApp.ViewModels.Booking
{
	public class BookingViewModel : IValidatableObject
	{
		public ApartViewModel Apart { get; set; }

		public string UserId { get; set; }
		[DisplayName("Начало бронирования")]
		[Required(ErrorMessage = "Не указано имя")]
		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }
		[DisplayName("Конец бронирования")]
		[Required(ErrorMessage = "Не указано имя")]
		[DataType(DataType.Date)]
		public DateTime ExpectedEndDate { get; set; }
		public DateTime? EndDate { get; set; }

		public BookingViewModel()
		{
			StartDate = DateTime.Now.AddDays(1);
			ExpectedEndDate = DateTime.Now.AddDays(8);
		}

		public BookingViewModel(BookingModel model)
		{
			if (model.Apartment != null)
				Apart = new ApartViewModel(model.Apartment);
			StartDate = model.StartDate;
			ExpectedEndDate = model.ExpectedEndTime;
			EndDate = model.EndTime;
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> errors = new List<ValidationResult>();

			if (StartDate > ExpectedEndDate)
			{
				errors.Add(new ValidationResult("Дата начала бронирования не может быть позже даты окончания", new List<string>() { "StartDate" }));
				errors.Add(new ValidationResult("Дата окончания бронирования не может быть раньше даты начала", new List<string>() { "ExpectedEndDate" }));
			}
			return errors;
		}
	}
}
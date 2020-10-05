using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.AdditionalValidation
{
    public class DateToAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            DateTime dateTo = Convert.ToDateTime(value);
            DateTime dateNow = DateTime.Now;
            return dateTo >= dateNow.AddDays(1).AddMinutes(-10); //Dates Greater than or equal to today are valid (true)

        }
    }
}

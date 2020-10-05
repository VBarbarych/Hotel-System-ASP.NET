using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.AdditionalValidation
{
    public class DateFromAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            DateTime dateFrom = Convert.ToDateTime(value);
            return dateFrom >= DateTime.Now.AddMinutes(-10); //Dates Greater than or equal to today are valid (true)

        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DTOs.Validation
{
    public class FutureOrPresentDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateValue = (DateTime?)value;

            if (dateValue.HasValue && dateValue.Value < DateTime.Today)
            {
                return false;
            }

            return true;
        }
    }
}

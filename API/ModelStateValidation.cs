using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API
{
    public static class ModelStateValidation
    {
        public static string GetValidationErrors(ModelStateDictionary modelState)
        {
            List<string> errors = new List<string>();
            foreach (var value in modelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return string.Join("\n", errors);
        }
    }
}

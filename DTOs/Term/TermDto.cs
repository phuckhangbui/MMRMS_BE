using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Term
{
    public class TermDto
    {
        public int TermId { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }

    public class CreateTermDto
    {
        [Required(ErrorMessage = MessageConstant.Machine.MachinePriceRequired)]
        public string Type { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.MachinePriceRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.MachinePriceRequired)]
        public string Content { get; set; }
    }

    public class UpdateTermDto
    {
        public int TermId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}

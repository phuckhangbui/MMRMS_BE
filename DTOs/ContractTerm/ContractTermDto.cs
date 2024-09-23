using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Term
{
    public class ContractTermDto
    {
        public int ContractTermId { get; set; }

        public string? ContractId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? DateCreate { get; set; }
    }

    public class ContractTermRequestDto
    {
        public string? Title { get; set; }

        public string? Content { get; set; }
    }
}

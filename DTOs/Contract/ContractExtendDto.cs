using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Contract
{
    public class ContractExtendDto
    {
        [Required(ErrorMessage = MessageConstant.Contract.DateEndRequired)]
        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }
    }
}

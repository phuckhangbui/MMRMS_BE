//using Common;
//using DTOs.Validation;
//using System.ComponentModel.DataAnnotations;

//namespace DTOs.Promotion
//{
//    public class PromotionDto
//    {
//        public int PromotionId { get; set; }
//        public string? DiscountTypeName { get; set; }
//        public double? DiscountPercentage { get; set; }
//        public string? Description { get; set; }
//        public string? Content { get; set; }
//        public DateTime? DateStart { get; set; }
//        public DateTime? DateEnd { get; set; }
//        public DateTime? DateCreate { get; set; }
//        public string? Status { get; set; }
//    }

//    public class PromotionRequestDto
//    {
//        [Required(ErrorMessage = MessageConstant.Promotion.DiscountTypeNameRequired)]
//        public string? DiscountTypeName { get; set; }

//        [Required(ErrorMessage = MessageConstant.Promotion.DiscountPercentageRequired)]
//        [Range(0, 100, ErrorMessage = MessageConstant.Promotion.DiscountPercentageRange)]
//        public double? DiscountPercentage { get; set; }

//        [Required(ErrorMessage = MessageConstant.Promotion.DescriptionRequired)]
//        public string? Description { get; set; }

//        [Required(ErrorMessage = MessageConstant.Promotion.ContentRequired)]
//        public string? Content { get; set; }

//        [Required(ErrorMessage = MessageConstant.Promotion.DateStartRequired)]
//        [FutureOrPresentDate(ErrorMessage = MessageConstant.Promotion.DateStartFutureOrPresent)]
//        public DateTime? DateStart { get; set; }

//        [Required(ErrorMessage = MessageConstant.Promotion.DateEndRequired)]
//        public DateTime? DateEnd { get; set; }
//    }
//}

namespace Common
{
    public static class MessageConstant
    {
        public const string InvalidStatusValue = "Giá trị trạng thái không hợp lệ";
        public const string ImageUploadError = "Lỗi tải ảnh lên";

        public static class Content
        {
            //Service
            public const string ContentListEmpty = "Danh sách nội dung trống";
            public const string ContentNotFound = "Nội dung không tồn tại";

            //DTO
            public const string TitleRequired = "Tiêu đề là bắt buộc.";
            public const string SummaryRequired = "Tóm tắt là bắt buộc.";
            public const string ContentRequired = "Nội dung là bắt buộc.";
            public const string ImageUrlRequired = "Đường dẫn hình ảnh là bắt buộc.";
        }

        public static class Category
        {
            //Service
            public const string CategoryNotFound = "Loại máy không tồn tại";
            public const string CategoryExistsError = "Loại máy với tên này đã tồn tại.";

            //DTO
            public const string CategoryNameRequired = "Tên loại máy là bắt buộc.";
        }

        public static class Account
        {
            //Service
            public const string InvalidRoleValue = "Giá trị vai trò không hợp lệ";
            public const string NotCustomerRole = "Tài khoản này không phải là vai trò khách hàng.";
            public const string NotStaffOrManagerRole = "Tài khoản này không phải là vai trò nhân viên hoặc quản lý.";
            public const string AccountNotFound = "Tài khoản không tồn tại.";
            public const string EmailAlreadyExists = "Tài khoản với email này đã tồn tại.";
            public const string UsernameAlreadyExists = "Tài khoản với tên người dùng này đã tồn tại.";
            public const string AccountDeleted = "Tài khoản đã bị xóa";
            public const string WrongPassword = "Sai mật khẩu";
            public const string AccountInactive = "Tài khoản chưa kích hoạt";
            public const string AccountLocked = "Tài khoản đã bị khóa";
            public const string WrongOtp = "Mã OTP không hợp lệ";

            //DTO
            public const string NameRequired = "Tên là bắt buộc.";
            public const string EmailRequired = "Email là bắt buộc.";
            public const string InvalidEmail = "Địa chỉ email không hợp lệ.";
            public const string AddressRequired = "Địa chỉ là bắt buộc.";
            public const string PhoneRequired = "Số điện thoại là bắt buộc.";
            public const string CitizenCardRequired = "Số chứng minh nhân dân là bắt buộc.";
            public const string GenderRequired = "Giới tính là bắt buộc.";
            public const string UsernameRequired = "Tên người dùng là bắt buộc.";
            public const string DateExpireRequired = "Ngày hết hạn là bắt buộc.";
            public const string DateBirthRequired = "Ngày sinh là bắt buộc.";
            public const string RoleIdRequired = "ID vai trò là bắt buộc.";
            public const string CompanyRequired = "Công ty là bắt buộc.";
            public const string PositionRequired = "Chức vụ là bắt buộc.";
            public const string TaxNumberRequired = "Số thuế là bắt buộc.";
            public const string BusinessTypeRequired = "Loại hình doanh nghiệp là bắt buộc.";
            public const string PasswordRequired = "Mật khẩu là bắt buộc.";
            public const string OtpRequired = "Mã OTP là bắt buộc.";
        }

        public static class MembershipRank
        {
            //Service
            public const string MembershipRanksEmpty = "Danh sách hạng thành viên trống.";
            public const string MembershipRankNotFound = "Hạng thành viên không tồn tại.";

            //DTO
            public const string MembershipRankNameRequired = "Tên hạng thành viên là bắt buộc.";
            public const string MoneySpentRequired = "Số tiền đã chi là bắt buộc.";
            public const string MoneySpentRange = "Số tiền đã chi phải là một số dương.";
            public const string DiscountPercentageRequired = "Phần trăm giảm giá là bắt buộc.";
            public const string DiscountPercentageRange = "Phần trăm giảm giá phải nằm trong khoảng từ 0 đến 100.";
            public const string ContentRequired = "Nội dung là bắt buộc.";
        }

        public static class Promotion
        {
            //Service
            public const string PromotionListEmpty = "Danh sách khuyến mãi trống.";
            public const string PromotionNotFound = "Khuyến mãi không tồn tại.";

            //DTO
            public const string DiscountTypeNameRequired = "Tên loại giảm giá là bắt buộc.";
            public const string DiscountPercentageRequired = "Phần trăm giảm giá là bắt buộc.";
            public const string DiscountPercentageRange = "Phần trăm giảm giá phải nằm trong khoảng từ 0 đến 100.";
            public const string DescriptionRequired = "Mô tả là bắt buộc.";
            public const string ContentRequired = "Nội dung là bắt buộc.";
            public const string DateStartRequired = "Ngày bắt đầu là bắt buộc.";
            public const string DateStartFutureOrPresent = "Ngày bắt đầu phải là hôm nay hoặc trong tương lai.";
            public const string DateEndRequired = "Ngày kết thúc là bắt buộc.";
        }

        public static class Product
        {
            //Service
            public const string ProductNotFound = "";
            public const string ProductNameDuplicated = "";
            public const string ProductModelDuplicated = "";
            public const string ComponentIdListNotCorrect = "";
            public const string StatusRequired = "";
            public const string StatusNotAvailable = "";



            //DTO
            public const string ProductNameRequired = "";
            public const string DescriptionRequired = "";
            public const string ProductPriceRequired = "";
            public const string RentPriceRequired = "";
            public const string ModelRequired = "";
            public const string OrginRequired = "";
            public const string CategoryRequired = "";

            public const string ProductPricePositiveNumber = "";
            public const string RentPricePositiveNumber = "";

        }

        public static class ProductAttribute
        {
            //Service


            //DTO
            public const string NameRequired = "";
            public const string SpecsRequired = "";
        }

        public static class Component
        {
            //Service
            public const string ComponetNameDuplicated = "";
            public const string ComponentNotExisted = "";


            //DTO
            public const string ComponentIdRequired = "";
            public const string ComponentNameRequired = "";
            public const string QuantityRequired = "";
            public const string QuantityPositiveNumber = "";
            public const string PriceRequired = "";
            public const string PricePositiveNumber = "";



        }

        public static class SerialNumberProduct
        {
            //Service
            public const string SerialNumberProductDuplicated = "";
            public const string ProductHaveNoComponentAndIsForceSetToFalse = "";


            //DTO
            public const string ProductIdRequired = "";
            public const string SerialNumberRequired = "";
            public const string ForceWhenNoComponentInProductRequired = "";

        }

        public static class Contract
        {
            //Service
            public const string ContractListEmpty = "Danh sách hợp đồng trống.";
            public const string ContractNotFound = "Hợp đồng không tồn tại.";
            public const string HiringRequestInvalid = "Yêu cầu thuê không hợp lệ.";
            public const string AccountRentInvalid = "Tài khoản thuê không hợp lệ.";
            public const string SerialNumberProductsInvalid = "Sản phẩm theo số sê-ri không hợp lệ.";

            //DTO
            public const string ContractNameRequired = "Tên hợp đồng là bắt buộc.";
            public const string AccountSignIdRequired = "Tài khoản ký là bắt buộc.";
            public const string AddressIdRequired = "Địa chỉ là bắt buộc.";
            public const string HiringRequestIdRequired = "Mã yêu cầu thuê là bắt buộc.";
            public const string ContentRequired = "Nội dung hợp đồng là bắt buộc.";
            public const string DateStartRequired = "Ngày bắt đầu là bắt buộc.";
            public const string DateStartFutureOrPresent = "Ngày bắt đầu phải là hôm nay hoặc trong tương lai.";
            public const string DateEndRequired = "Ngày kết thúc là bắt buộc.";
            public const string DateEndAfterStart = "Ngày kết thúc phải sau ngày bắt đầu.";
            public const string ContractTermsRequired = "Danh sách điều khoản hợp đồng là bắt buộc.";
            public const string SerialNumberProductsRequired = "Danh sách sản phẩm với số serial là bắt buộc.";
        }
    }
}

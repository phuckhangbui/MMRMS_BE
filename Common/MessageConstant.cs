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
            public const string GenderRequired = "Giới tính là bắt buộc.";
            public const string UsernameRequired = "Tên người dùng là bắt buộc.";
            public const string DateExpireRequired = "Ngày hết hạn là bắt buộc.";
            public const string DateBirthRequired = "Ngày sinh là bắt buộc.";
            public const string RoleIdRequired = "ID vai trò là bắt buộc.";
            public const string CompanyRequired = "Công ty là bắt buộc.";
            public const string PositionRequired = "Chức vụ là bắt buộc.";
            public const string TaxNumberRequired = "Số thuế là bắt buộc.";
            public const string PasswordRequired = "Mật khẩu là bắt buộc.";
            public const string OtpRequired = "Mã OTP là bắt buộc.";
        }

        public static class MembershipRank
        {
            //Service
            public const string MembershipRanksEmpty = "Danh sách hạng thành viên trống.";
            public const string MembershipRankNotFound = "Hạng thành viên không tồn tại.";
            public const string NoMembershipRank = "Khách hàng hiện không có hạng thành viên.";

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
            public const string ProductNotFound = "Không tìm thấy sản phẩm";
            public const string ProductNameDuplicated = "Tên sản phẩm bị trùng";
            public const string ProductModelDuplicated = "Mã model sản phẩm bị trùng";
            public const string ComponentIdListNotCorrect = "Danh sách bộ phận máy không đúng";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";
            public const string ProductHasSerialNumberCannotDeleted = "Sản phẩm này đã có danh sách các mã serial, vì vậy không thể xóa";
            public const string ProductHasSerialNumberCannotUpdateComponentList = "Sản phẩm này đã có danh sách các mã serial, vì vậy không thể cập nhật danh sách các bộ phận máy";


            //DTO
            public const string ProductNameRequired = "Tên máy là bắt buộc";
            public const string DescriptionRequired = "Miêu tả máy là bắt buộc";
            public const string ProductPriceRequired = "Giá máy là bắt buộc";
            public const string RentPriceRequired = "Giá thuê là bắt buộc";
            public const string ModelRequired = "Mã model là bắt buộc";
            public const string OrginRequired = "Nguồn gốc là bắt buộc";
            public const string CategoryRequired = "Loại máy là bắt buộc";
            public const string StatusRequired = "Trạng thái là bắt buộc";

            public const string ProductPricePositiveNumber = "Giá máy phải là số dương";
            public const string RentPricePositiveNumber = "Giá thuê phải là số dương";
            public const string ImageIsRequired = "Hình ảnh của sản phẩm là bắt buộc";

        }

        public static class ProductAttribute
        {
            //Service


            //DTO
            public const string NameRequired = "Tên là bắt buộc";
            public const string SpecsRequired = "Chi tiết máy là bắt buộc";
        }

        public static class ProductTerm
        {
            //Service


            //DTO
            public const string TitleRequired = "Tiêu đề là bắt buộc";
            public const string ContentRequired = "Nội dung là bắt buộc";
        }



        public static class Component
        {
            //Service
            public const string ComponetNameDuplicated = "Tên bộ phận máy bị trùng";
            public const string ComponentNotExisted = "Bộ phận máy không tồn tại";
            public const string ComponentHasBeenUsedCannotUpdateName = "Bộ phận máy đã được sử dụng, không thể đổi tên";
            public const string ComponentStatusCannotSet = "Tình trạng bộ phận máy này không thể được cài đặt";
            public const string ComponentHasBeenUsedCannotDelete = "Bộ phận máy đã được sử dụng, không thể xóa";


            //DTO
            public const string ComponentIdRequired = "ID bộ phận máy là bắt buộc";
            public const string ComponentNameRequired = "Tên bộ phận máy là bắt buộc";
            public const string ComponentStatusRequired = "Tình trạng máy là bắt buộc";
            public const string QuantityRequired = "Số lượng là bắt buộc";
            public const string QuantityPositiveNumber = "Số lượng phải là số dương";
            public const string PriceRequired = "Giá bộ phận máy là bắt buộc";
            public const string PricePositiveNumber = "Giá bộ phận máy phải là số dương";
            public const string IsRequiredMoneyRequire = "Bộ phận máy này cần phải trả tiền khi thay thế hay không là bắt buộc";



        }

        public static class SerialNumberProduct
        {
            //Service
            public const string SerialNumberProductDuplicated = "Mã máy bị trùng";
            public const string ProductHaveNoComponentAndIsForceSetToFalse = "Máy này chưa có bộ phận, bạn có chắc là muốn thêm mã máy này?";
            public const string SerialNumberProductNotFound = "Mã máy không tồn tại";
            public const string SerialNumberProductHasContract = "Mã máy đã có trong hợp đồng, không thể xóa";
            public const string StatusCannotSet = "Tình trạng mã máy này không thể được cài đặt";
            public const string NoAvailableSerailNumberProductForRenting = "Không có sản phẩm với số serial khả dụng để cho thuê.";

            //DTO
            public const string ProductIdRequired = "ID máy là bắt buộc";
            public const string SerialNumberRequired = "Mã máy là bắt buộc";
            public const string ForceWhenNoComponentInProductRequired = "Thêm flag khi sản phẩm có hoặc không có bộ phận máy";

            public const string ActualRentPriceRequired = "Giá thuê máy là bắt buộc";
            public const string RentTimeCounterequired = "Số lần máy đã cho thuê là bắt buộc";

        }

        public static class Contract
        {
            public const string SignContractSuccessfully = "Ký hợp đồng thành công";

            //Service
            public const string ContractListEmpty = "Danh sách hợp đồng trống.";
            public const string ContractNotFound = "Hợp đồng không tồn tại.";
            public const string RentingRequestInvalid = "Yêu cầu thuê không hợp lệ.";
            public const string AccountRentInvalid = "Tài khoản thuê không hợp lệ.";
            public const string SerialNumberProductsInvalid = "Sản phẩm theo số sê-ri không hợp lệ.";
            public const string ContractOutOfRange = "Hợp đồng này chưa bắt đầu hoặc là đã kết thúc";

            //DTO
            public const string ContractNameRequired = "Tên hợp đồng là bắt buộc.";
            public const string AccountSignIdRequired = "Tài khoản ký là bắt buộc.";
            public const string AddressIdRequired = "Địa chỉ là bắt buộc.";
            public const string RentingRequestIdRequired = "Mã yêu cầu thuê là bắt buộc.";
            public const string ContentRequired = "Nội dung hợp đồng là bắt buộc.";
            public const string DateStartRequired = "Ngày bắt đầu là bắt buộc.";
            public const string DateStartFutureOrPresent = "Ngày bắt đầu phải là hôm nay hoặc trong tương lai.";
            public const string DateEndRequired = "Ngày kết thúc là bắt buộc.";
            public const string DateEndAfterStart = "Ngày kết thúc phải sau ngày bắt đầu.";
            public const string ContractTermsRequired = "Danh sách điều khoản hợp đồng là bắt buộc.";
            public const string SerialNumberProductsRequired = "Danh sách sản phẩm với số serial là bắt buộc.";
        }

        public static class Notification
        {
            //Service
            public const string NotificationNotFound = "Thông báo này không tồn tại";

        }

        public static class RentingRequest
        {
            //Service
            public const string RequestProductsInvalid = "Danh sách máy yêu cầu thuê không hợp lệ";
            public const string RequestAccountInvalid = "Tài khoản yêu cầu thuê không hợp lệ.";
            public const string RequestAddressInvalid = "Địa chỉ yêu cầu thuê không hợp lệ.";
            public const string RentingRequestNotFound = "Yêu cầu thuê không tồn tại";
            public const string RentingRequestCanNotCancel = "Yêu cầu thuê không tồn tại hoặc không thể hủy";
            public const string RentingRequestCancelSuccessfully = "Hủy yêu cầu thuê thành công";
            public const string RentingRequestCancelFail = "Hủy yêu cầu thuê thất bại";

            //DTO
            public const string AddressIdRequired = "Địa chỉ là bắt buộc.";
            public const string DateStartRequired = "Ngày bắt đầu là bắt buộc.";
            public const string DateStartFutureOrPresent = "Ngày bắt đầu phải là hôm nay hoặc trong tương lai.";
            public const string TotalRentPriceRequired = "Tổng giá thuê là bắt buộc.";
            public const string TotalDepositPriceRequired = "Tổng tiền đặt cọc là bắt buộc.";
            public const string ShippingPriceRequired = "Phí vận chuyển là bắt buộc.";
            public const string NumberOfMonthRequired = "Số tháng thuê là bắt buộc.";
            public const string TotalAmountRequired = "Tổng tiền là bắt buộc.";
            public const string IsOnetimePaymentRequired = "Hình thức thanh toán một lần là bắt buộc.";
            public const string RequestProductsRequired = "Danh sách máy yêu cầu thuê là bắt buộc.";
            public const string ServiceRentingRequestsRequired = "Danh sách dịch vụ thuê là bắt buộc.";
        }

        public static class RentingService
        {
            //Service
            public const string RentingServiceListEmpty = "Danh sách dịch vụ thuê trống";
            public const string RentingServiceNotFound = "Dịch vụ thuê không tồn tại";
            public const string RentingServiceCanNotDelete = "Dịch vụ thuê này đã được sử dụng và không thể xóa.";

            //DTO
            public const string RentingServiceNameRequired = "Tên dịch vụ thuê là bắt buộc.";
            public const string DescriptionRequired = "Mô tả là bắt buộc.";
        }

        public static class AccountPromotion
        {
            //Service
            public const string AccountPromotionListEmpty = "Danh sách khuyến mãi của bạn trống";
        }

        public static class Address
        {
            //Service
            public const string AddressListEmpty = "Danh sách địa chỉ của bạn trống";

            //DTO
            public const string AddressBodyRequired = "Địa chỉ chi tiết là bắt buộc.";
            public const string DistrictRequired = "Quận/huyện là bắt buộc.";
            public const string CityRequired = "Thành phố là bắt buộc.";
        }

        public static class Delivery
        {
            //Service
            public const string DeliveryNotFound = "Mã giao hàng này không tồn tại";
            public const string StatusCannotSet = "Tình trạng giao hàng này không thể được cài đặt";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";


            //DTO
            public const string DeliveryIdRequired = "Mã giao hàng là bắt buộc";
            public const string StaffIdRequired = "Mã nhân viên là bắt buộc";
            public const string DateshipIsRequired = "Ngày giao là bắt buộc";

        }

        public static class MaintenanceRequest
        {
            //Service
            public const string RequestNotFound = "Mã yêu cầu này không tồn tại";
            public const string StatusCannotSet = "Tình trạng yêu cầu này không thể được cài đặt";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";


            //DTO
        }

        public static class EmployeeTask
        {
            //Service
            public const string TaskNotFound = "Mã công việc này không tồn tại";
            public const string StatusCannotSet = "Tình trạng công việc này không thể được cài đặt";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";
            public const string CannotDeleted = "Công việc này không thể xóa";
            public const string ReachMaxTaskLimit = "Nhân viên này đã đạt đến số công việc tối đa trong ngày";

            //DTO
            public const string RequestIdRequired = "Mã yêu cầu là bắt buộc";
            public const string StaffIdRequired = "Mã nhân viên là bắt buộc";
            public const string TaskContentRequired = "Nội dung công việc là bắt buộc";
            public const string TitleRequired = "Tên công việc là bắt buộc";
            public const string DateStartRequired = "Ngày làm việc là bắt buộc";

        }

        public static class MaintanningTicket
        {
            //Service
            public const string TicketNotFound = "Mã yêu cầu thay thế này không tồn tại";
            public const string StatusCannotSet = "Tình trạng yêu cầu thay thế này không thể được cài đặt";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";

            //DTO
            public const string ComponentIdRequired = "Mã bộ phận máy là bắt buộc";
            public const string ProductSerialNumberRequired = "Mã máy là bắt buộc";
            public const string ContractIdRequired = "Mã hợp đồng là bắt buộc";
            public const string PriceRequired = "Giá tiền là bắt buộc";
            public const string PricePositiveNumberRequired = "Giá tiền phải là số dương";
            public const string QuantityRequired = "Số lượng là bắt buộc";
            public const string QuantityPositiveNumberRequired = "Số lượng phải là số dương";
            public const string AdditionFeeRequired = "Chi phí phụ thu là bắt buộc";
            public const string TypeRequired = "Loại là bắt buộc";
            public const string NoteRequired = "Ghi chú là bắt buộc";


        }

        public static class Term
        {
            //Service
            public const string TermNotFound = "Điều khoản này không tìm thấy";
            public const string TermTypeNotCorrect = "Loại điều khoản này không tồn tại";


            //DTO
            public const string TermIdRequired = "Mã điều khoản là bắt buộc";
            public const string TypeRequired = "Loại điều khoản là bắt buộc";
            public const string TitleRequired = "Tiêu đề điều khoản là bắt buộc";
            public const string ContentRequired = "Nội dung điều khoản là bắt buộc";
        }

        public static class Invoice
        {
            //Service
            public const string InvoiceNotFound = "Hóa đơn này không tồn tại";
            public const string IncorrectAccountIdForInvoice = "Hóa đơn này không thể được trả bởi bạn";
            public const string InvoiceHaveBeenPaid = "Hóa đơn này đã được thanh toán";

            //DTO
            public const string ReturnUrlRequired = "Đường dẫn trả về là bắt buộc";
            public const string CancelUrlRequired = "Đường dẫn hủy đơn thanh toán là bắt buộc";
        }

        public static class PayOS
        {
            //Service
            public const string PaymentReferenceError = "Đã có lỗi xảy ra trong quá trình thực hiện thanh toán. Hiện tại trạng thái của mã này đang là: ";
        }
    }
}

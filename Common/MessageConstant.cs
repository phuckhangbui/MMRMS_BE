﻿namespace Common
{
    public static class MessageConstant
    {
        public const string InvalidStatusValue = "Giá trị trạng thái không hợp lệ";
        public const string ImageUploadError = "Lỗi tải ảnh lên";

        public static class Content
        {
            //Service
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
            //Controller
            public const string UpdateAccountSuccessfully = "Cập nhật tài khoản thành công";
            public const string UpdateAccountFail = "Cập nhật tài khoản thất bại";
            public const string ChangeAccountStatusSuccessfully = "Cập nhật trạng thái tài khoản thành công";
            public const string ChangeAccountStatusFail = "Cập nhật trạng thái tài khoản thất bại";

            //Service
            public const string InvalidRoleValue = "Giá trị vai trò không hợp lệ";
            public const string AccountNotFound = "Tài khoản không tồn tại.";
            public const string EmailAlreadyExists = "Tài khoản với email này đã tồn tại.";
            public const string UsernameAlreadyExists = "Tài khoản với tên người dùng này đã tồn tại.";
            public const string AccountDeleted = "Tài khoản đã bị xóa";
            public const string WrongPassword = "Sai mật khẩu";
            public const string AccountInactive = "Tài khoản chưa kích hoạt";
            public const string AccountLocked = "Tài khoản đã bị khóa";
            public const string AccountPendingConfirm = "Tài khoản chưa được hệ thống thông qua, vui lòng chờ chúng tôi xác nhận lại một số thông tin";
            public const string AccountChosenLocked = "Tài khoản bạn chọn đã bị khóa";
            public const string WrongOtp = "Mã OTP không hợp lệ";
            public const string AccountNotValidToUpdate = "Tài khoản không thể cập nhật do thông tin không hợp lệ";
            public const string AccountRoleIsNotSuitableToAssignForThisTask = "Tài khoản nhân viên mà bạn chọn không thể thực hiện chức năng này";
            public const string AccountStatusNotSuitableForApproval = "Tài khoản của khách hàng này hiện đang không thể thực hiện được thao tác này";
            public const string DateBirthInvalid = "Ngày sinh không hợp lệ. Phải lớn hơn 18 và nhỏ hơn 120 tuổi";


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
            public const string AvatarUrlRequired = "Avatar là bắt buộc.";
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

        public static class Machine
        {
            //Service
            public const string MachineNotFound = "Không tìm thấy sản phẩm";
            public const string MachineNameDuplicated = "Tên sản phẩm bị trùng";
            public const string MachineModelDuplicated = "Mã model sản phẩm bị trùng";
            public const string ComponentIdListNotCorrect = "Danh sách bộ phận máy không đúng";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";
            public const string MachineHasSerialNumberCannotDeleted = "Sản phẩm này đã có danh sách các mã serial, vì vậy không thể xóa";
            public const string MachineHasSerialNumberCannotUpdateComponentList = "Sản phẩm này đã có danh sách các mã serial, vì vậy không thể cập nhật danh sách các bộ phận máy";
            public const string MachineStateNotSuitableForModifyStatus = "Trạng thái máy hiện giờ không cho phép khóa/mở khóa";


            //DTO
            public const string MachineNameRequired = "Tên máy là bắt buộc";
            public const string DescriptionRequired = "Miêu tả máy là bắt buộc";
            public const string MachinePriceRequired = "Giá máy là bắt buộc";
            public const string RentPriceRequired = "Giá thuê là bắt buộc";
            public const string ModelRequired = "Mã model là bắt buộc";
            public const string OrginRequired = "Nguồn gốc là bắt buộc";
            public const string CategoryRequired = "Loại máy là bắt buộc";
            public const string StatusRequired = "Trạng thái là bắt buộc";

            public const string MachinePricePositiveNumber = "Giá máy phải là số dương";
            public const string RentPricePositiveNumber = "Giá thuê phải là số dương";
            public const string ImageIsRequired = "Hình ảnh của sản phẩm là bắt buộc";
            public const string ShipPriceRequired = "Giá tiền giao hàng là bắt buộc";
            public const string WeightRequired = "Cân nặng là bắt buộc";
        }

        public static class MachineAttribute
        {
            //Service


            //DTO
            public const string NameRequired = "Tên là bắt buộc";
            public const string SpecsRequired = "Chi tiết máy là bắt buộc";
        }

        public static class MachineTerm
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

        public static class MachineSerialNumber
        {
            //Service
            public const string MachineSerialNumberDuplicated = "Mã máy bị trùng";
            public const string MachineHaveNoComponentAndIsForceSetToFalse = "Máy này chưa có bộ phận, bạn có chắc là muốn thêm mã máy này?";
            public const string MachineSerialNumberNotFound = "Mã máy không tồn tại";
            public const string MachineSerialNumberHasContract = "Mã máy đã có trong hợp đồng, không thể xóa";
            public const string StatusCannotSet = "Tình trạng mã máy này không thể được cài đặt";
            public const string MachineStateNotSuitableForModifyStatus = "Trạng thái máy hiện giờ không cho phép khóa/mở khóa";
            public const string ComponentIdNotFound = "Mã bộ phận máy serial này không tìm thấy";
            public const string ComponentIsNotBrokenToCreateTicket = "Bộ phận máy này chưa hư hỏng để tạo ticket";
            public const string YouCannotUpdateStatusOfComponentWhileRenting = "Bạn không thể cập nhật trạng thái của bộ phận máy khi máy đang thuê";
            public const string InvalidQuantity = "Số lượng bộ phận bạn nhập vào không phù hợp với máy này";
            public const string MachineNotSuitableForAvailableStatus = "Trạng thái máy hiện tại không thể chuyển sang sẵn sàng";
            public const string MachineNotSuitableForMaintenanceStatus = "Trạng thái máy hiện tại không thể chuyển sang đang bảo trì";
            public const string MachineComponentStillBroken = "Thành phần máy hiện tại vẫn còn hư hỏng, chưa thể chuyển trạng thái máy qua sẵn sàng";
            public const string MachineSerialNumberNotMappingCorrect = "Bộ phận máy serial này không thuộc về máy này";
            public const string PercentHigherThanSixty = "Tình trạng máy cần lớn hơn 60%";
            public const string PercentIsNotDivisibleByFive = "Tình trạng máy cần chia hết cho 5";


            //DTO
            public const string MachineIdRequired = "ID máy là bắt buộc";
            public const string SerialNumberRequired = "Mã máy là bắt buộc";
            public const string ForceWhenNoComponentInMachineRequired = "Thêm flag khi sản phẩm có hoặc không có bộ phận máy";

            public const string ActualRentPriceRequired = "Giá thuê máy là bắt buộc";
            public const string RentTimeCounterequired = "Số lần máy đã cho thuê là bắt buộc";
            public const string StatusRequired = "Trạng thái của máy là bắt buộc";
            public const string MachineConditionPercentRequired = "Độ mới của máy là bắt buộc";
        }

        public static class Contract
        {
            //Controller
            public const string EndContractSuccessfully = "Kết thúc hợp đồng thành công";
            public const string EndContractFail = "Kết thúc hợp đồng thất bại";
            public const string ExtendContractSuccessfully = "Gia hạn hợp đồng thành công";
            public const string ExtendContractFail = "Gia hạn hợp đồng thất bại";

            //Service
            public const string ContractNotFound = "Hợp đồng không tồn tại.";
            public const string ContractOutOfRange = "Hợp đồng này chưa bắt đầu hoặc là đã kết thúc";
            public const string ContractIsNotReadyForRequest = "Hợp đồng này chưa thể tạo yêu cầu liên quan";
            public const string ContractNotValidToDelivery = "Hợp đồng không thể giao do chưa ký hoặc đã giao thành công, mã hợp đồng: ";
            public const string ContractNotValidToDeliveryOldContractDeliveryStillActive = "Hợp đồng không thể giao do vẫn còn lịch cũ chưa hoàn thành/ đã thành công, mã hợp đồng: ";
            public const string ContractNotValidToEnd = "Hợp đồng không thể kết thúc do không hợp lệ";
            public const string ContractNotValidToCreateRefundInvoice = "Hợp đồng không hợp lệ để tạo hóa đơn hoàn trả.";
            public const string ExtensionPeriodNotValid = "Thời gian gia hạn phải tối thiểu 30 ngày tính từ ngày kết thúc của hợp đồng hiện tại.";
            public const string ContractNotValidToExtend = "Hợp đồng không thể gia hạn do không hợp lệ";
            public const string ContractDeliveryNotFound = "Mã số đơn giao hợp đồng không tìm thấy";
            public const string ContractAlreadyExtended = "Hợp đồng này đã được gia hạn, không thể gia hạn được nữa";
            public const string ContractNotBelongToThisAccount = "Hợp đồng này không thuộc tài khoản đăng nhập hiện tại";
            public const string ExtendContractNotValidToCancel = "Hợp đồng gia hạn này không thể hủy do đã ký";
            public const string ContractNotBelongToCurrentAccount = "Hợp đồng không thuộc tài khoản hiện tại.";
            public const string ContractCanNotEnd = "Hợp đồng không thể hủy do hợp đồng gia hạn đã ký";

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
            public const string MachineSerialNumbersRequired = "Danh sách sản phẩm với số serial là bắt buộc.";
        }

        public static class Notification
        {
            //Service
            public const string NotificationNotFound = "Thông báo này không tồn tại";

        }

        public static class RentingRequest
        {
            //Service
            public const string RequestMachinesInvalid = "Danh sách máy yêu cầu thuê không hợp lệ";
            public const string RequestAddressInvalid = "Địa chỉ yêu cầu thuê không hợp lệ.";
            public const string RentPeriodInValid = "Thời gian yêu cầu thuê không hợp lệ";
            public const string RentingRequestNotFound = "Yêu cầu thuê không tồn tại";
            public const string RentingRequestCanNotCancel = "Yêu cầu thuê không tồn tại hoặc không thể hủy";
            public const string RentingRequestCancelSuccessfully = "Hủy yêu cầu thuê thành công";
            public const string RentingRequestCancelFail = "Hủy yêu cầu thuê thất bại";
            public const string CreateRentingRequestFail = "Tạo yêu cầu thuê thất bại";

            //DTO
            public const string AddressIdRequired = "Địa chỉ là bắt buộc.";
            public const string DateStartRequired = "Ngày bắt đầu là bắt buộc.";
            public const string DateEndRequired = "Ngày kết thúc là bắt buộc.";
            public const string DateFutureOrPresent = "Ngày phải là hôm nay hoặc trong tương lai.";
            public const string TotalRentPriceRequired = "Tổng giá thuê là bắt buộc.";
            public const string TotalDepositPriceRequired = "Tổng tiền đặt cọc là bắt buộc.";
            public const string ShippingPriceRequired = "Phí vận chuyển là bắt buộc.";
            public const string NumberOfMonthRequired = "Số tháng thuê là bắt buộc.";
            public const string TotalAmountRequired = "Tổng tiền là bắt buộc.";
            public const string IsOnetimePaymentRequired = "Hình thức thanh toán một lần là bắt buộc.";
            public const string RequestMachinesRequired = "Danh sách máy yêu cầu thuê là bắt buộc.";
            public const string ServiceRentingRequestsRequired = "Danh sách dịch vụ thuê là bắt buộc.";
            public const string MachineIdRequired = "Mã máy là bắt buộc.";
            public const string SerialRequired = "Serial là bắt buộc.";
            public const string AccountNumberRequired = "Số tài khoản ngân hàng là bắt buộc";
            public const string BeneficiaryBankRequired = "Tên ngân hàng thụ hưởng là bắt buộc";
            public const string BeneficiaryNameRequired = "Tên người thụ hưởng là bắt buộc";
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

        public static class Address
        {
            //Controller
            public const string ChangeAddressSuccessfully = "Cập nhật địa chỉ thành công";
            public const string ChangeAddressFail = "Cập nhật địa chỉ thất bại";
            public const string DeleteAddressSuccessfully = "Xóa địa chỉ thành công";
            public const string DeleteAddressFail = "Xóa địa chỉ thất bại";

            //Service
            public const string AddressListEmpty = "Danh sách địa chỉ của bạn trống";
            public const string AddressNotValid = "Địa chỉ không tồn tại hoặc không hợp lệ";

            //DTO
            public const string AddressBodyRequired = "Địa chỉ chi tiết là bắt buộc.";
            public const string DistrictRequired = "Quận/huyện là bắt buộc.";
            public const string CityRequired = "Thành phố là bắt buộc.";
        }

        public static class DeliveryTask
        {
            //Service
            public const string DeliveryTaskNotFound = "Mã giao hàng này không tồn tại";
            public const string StatusCannotSet = "Tình trạng giao hàng này không thể được cài đặt";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";
            public const string ContractAreNotInTheSameRequest = "Những hợp đồng này không cùng một đơn yêu cầu";
            public const string YouCannotChangeThisDelivery = "Bạn không có quyền thay đổi trạng thái của yêu cầu giao nhận này";
            public const string InvalidContractDeliveryList = "Danh sách các hợp đồng của lần giao hàng này không hợp lệ";
            public const string AllContractDeliveryFailPleaseCallAllFailAPI = "Tất cả các hợp đồng giao đều thất bại, xin hãy gọi API khác";
            public const string ContractDeliveryNotFound = "Mã hợp đồng giao hàng này không tìm thấy";
            public const string VehicleIsBiggerThanNumberOfMachine = "Số lượng xe đang lớn hơn số lượng máy";
            public const string MachineSerialNumberNotInAvailableStatus = "Máy này chưa thể giao do đang ở trạng thái: ";
            public const string ImageIsRequired = "Ảnh là bắt buộc";

            //DTO
            public const string DeliveryTaskIdRequired = "Mã giao hàng là bắt buộc";
            public const string StaffIdRequired = "Mã nhân viên là bắt buộc";
            public const string DateshipIsRequired = "Ngày giao là bắt buộc";
            public const string ContractIdListRequired = "Danh sách các mã contract cần giao là bắt buộc";
            public const string ReceiverNameRequired = "Tên người nhận là bắt buộc";
            public const string ContractDeliveryListRequired = "Danh sách các hợp đồng của lần giao hàng này là bắt buộc";
            public const string NoteRequired = "Ghi chú là bắt buộc";
            public const string ContractDeliveryIsSuccessRequired = "Trạng thái của từng đơn giao trong hợp đồng là bắt buộc";
            public const string DeliveryVehicleCounterRequired = "Số lượng xe của lần giao hàng là bắt buộc";
        }

        public static class MachineCheckRequest
        {
            //Service
            public const string RequestNotFound = "Mã yêu cầu này không tồn tại";
            public const string StatusCannotSet = "Tình trạng yêu cầu này không thể được cài đặt";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";
            public const string PendingRequestStillExist = "Bạn vẫn còn một yêu cầu chưa được xử lý, không thể tạo yêu cầu mới";
            public const string NotCorrectCustomer = "Bạn không thể thực hiện hành động này do đây không phải là yêu cầu của bạn";
            public const string RequestCannotCancel = "Yêu cầu này không thể hủy do quá trình sửa máy đã bắt đầu";
            public const string CancelRequestFail = "Quá trình hủy yêu cầu này đã xảy ra lỗi";

            //DTO
            public const string CriteriaNameRequired = "Tên tiêu chí kiểm tra máy là bắt buộc";

        }

        public static class MachineTask
        {
            //Service
            public const string TaskNotFound = "Mã công việc này không tồn tại";
            public const string StatusCannotSet = "Tình trạng công việc này không thể được cài đặt";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";
            public const string CannotDeleted = "Công việc này không thể xóa";
            public const string ReachMaxTaskLimit = "Nhân viên này đã đạt đến số công việc tối đa trong ngày";
            public const string TaskNotPossibleRequestStatus = "Yêu cầu này đã được xử lý, không thể tạo nhiệm vụ mới";
            public const string TaskTerminationNotPossibleContractStatus = "Hợp đồng này chưa thể tạo yêu cầu check máy đáo hạn hợp đồng";
            public const string TaskCheckDeliveryFailNotPossibleContractStatus = "Hợp đồng này chưa thể tạo yêu cầu check máy sau khi giao thất bại";
            public const string TaskNotPossibleComponentReplacementTicketStatus = "Chưa thể tạo nhiệm vụ mới với ticket này";
            public const string NotCorrectTaskType = "Loại công việc này không đúng để thực hiện chức năng này";
            public const string IncorrectStaffIdToUpdate = "Bạn không thể thực hiện hành động này";
            public const string CannotCreateTicketWithThisTask = "Trạng thái nhiệm vụ này không thể tạo ticket thay thế bộ phận máy";
            public const string CreateFail = "Có lỗi xảy ra trong quá trình tạo nhiệm vụ";
            public const string TaskCannotCompleteDueToTicketListUnfulfill = "Có lỗi xảy ra trong quá trình tạo nhiệm vụ";
            public const string TaskShipFailCannotCompleteDueToMachineStillHaveBrokenComponent = "Nhiệm vụ kiểm tra máy này chưa thể hoàn thành để đem máy đi giao được do có bộ phận bị hư hỏng";

            //DTO
            public const string RequestIdRequired = "Mã yêu cầu là bắt buộc";
            public const string ComponentReplacementTicketIdRequired = "Mã ticket thay thế bộ phận máy là bắt buộc";
            public const string StaffIdRequired = "Mã nhân viên là bắt buộc";
            public const string TaskContentRequired = "Nội dung công việc là bắt buộc";
            public const string TitleRequired = "Tên công việc là bắt buộc";
            public const string DateStartRequired = "Ngày làm việc là bắt buộc";
            public const string ContractIdRequired = "Mã hợp đồng là bắt buộc";
            public const string ContractDeliveryIdRequire = "Mã giao hợp đồng là bắt buộc";

        }

        public static class ComponentReplacementTicket
        {
            //Service
            public const string TicketNotFound = "Mã yêu cầu thay thế này không tồn tại";
            public const string StatusCannotSet = "Tình trạng yêu cầu thay thế này không thể được cài đặt";
            public const string StatusNotAvailable = "Trạng thái này không tồn tại";
            public const string BiggerQuantityThanMachine = "Số lượng nhập vào lớn hơn số lượng bộ phận của máy";
            public const string NotEnoughQuantity = "Bộ phận này hiện nay không có trong kho";
            public const string CreateFail = "Tạo ticket thay thế bộ phận máy thất bại";
            public const string NotReadyToBeCompletedWhenNotPaid = "Ticket này chưa thể hoàn thành do chưa thanh toán";
            public const string NotCorrectStaffId = "Tài khoản này không phải là người tạo ticket, do đó không thể hoàn thành nó";
            public const string CompleteFail = "Cập nhật trạng thái hoàn thành của ticket thất bại";
            public const string CannotCancelAlreadyPaid = "Không thể hủy ticket này do đã thanh toán";
            public const string NotCorrectCustomerId = "Bạn không thể hủy ticket này do đây không phải là ticket của bạn";
            public const string ErrorWhenCancel = "Có lỗi xảy ra khi hủy ticket";
            public const string DuplicateComponentTicketForThisTask = "Công việc này đã có một ticket thay thế bộ phận này, vì thế không thể tạo thêm";
            public const string DuplicateComponentTicketForThisContractWhenTerminate = "Hợp đồng này đã có một ticket thay thế bộ phận này khi đáo hạn, vì thế không thể tạo thêm";
            public const string MissingNoteWhenCancel = "Thiếu ghi chú khi nhân viên hủy công việc";


            //DTO
            public const string MachineSerialNumberComponentIdRequired = "Mã bộ phận máy của máy serial là bắt buộc";
            public const string MachineSerialNumberRequired = "Mã máy là bắt buộc";
            public const string ContractIdRequired = "Mã hợp đồng là bắt buộc";
            public const string PriceRequired = "Giá tiền là bắt buộc";
            public const string PricePositiveNumberRequired = "Giá tiền phải là số dương";
            public const string QuantityRequired = "Số lượng là bắt buộc";
            public const string QuantityPositiveNumberRequired = "Số lượng phải là số dương";
            public const string AdditionFeeRequired = "Chi phí phụ thu là bắt buộc";
            public const string TypeRequired = "Loại là bắt buộc";
            public const string NoteRequired = "Ghi chú là bắt buộc";
            public const string MachineTaskIdRequired = "Mã công việc máy là bắt buộc";


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
            //Controller
            public const string PayInvoiceSuccessfully = "Thanh toán hóa đơn thành công";
            public const string PayInvoiceFail = "Thanh toán hóa đơn thất bại";

            //Service
            public const string InvoiceNotFound = "Hóa đơn này không tồn tại";
            public const string IncorrectAccountIdForInvoice = "Hóa đơn này không thể được trả bởi bạn";
            public const string InvoiceHaveBeenPaid = "Hóa đơn này đã được thanh toán";
            public const string InvoiceHaveBeenCanceled = "Hóa đơn này đã bị hủy";
            public const string CreateInvoiceFail = "Tạo hóa đơn thất bại";

            //DTO
            public const string ReturnUrlRequired = "Đường dẫn trả về là bắt buộc";
            public const string CancelUrlRequired = "Đường dẫn hủy đơn thanh toán là bắt buộc";
            public const string ContractIdRequired = "Mã hợp đồng để tạo hóa đơn refund là bắt buộc";
            public const string InvoiceAmountRequired = "Số tiền cần trả của hóa đơn là bắt buộc";
            public const string InvoiceAmountPositive = "Số tiền cần trả của hóa đơn là số dương";
        }

        public static class PayOS
        {
            //Service
            public const string PaymentReferenceError = "Đã có lỗi xảy ra trong quá trình thực hiện thanh toán. Hiện tại trạng thái của mã này đang là: ";
        }

        public static class DashBoard
        {
            public const string FilterParamNotValid = "Tham số lọc không hợp lệ";
        }
    }
}

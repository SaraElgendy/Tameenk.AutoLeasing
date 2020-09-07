

using Newtonsoft.Json;

namespace Tameenk.AutoLeasing.Identity.Domain
{
    public class Output<TResult>
    {
        public enum ErrorCodes
        {
            Success = 1,
            EmptyInputParamter = 2,
            ServiceDown = 3,
            InvalidCaptcha = 4,
            ServiceException = 5,
            ServerException = 6,
            NotSuccess = 7,
            NotFound = 8,
            NotCorrect = 9,
            NotValid = 10,
            PhoneNumberNotConfirmed = 11,
            EmptyReturnObject = 12,
            InvalidRetrunObject = 13,
            InvalidIBAN = 14,
            InvalildInputParameter = 15,
            ErrorWithRedirectSearchResult = 16,
            FailedToAddItemToCart = 17,
            InvalidQuotation = 18,
            InvalidQuotationNumberOfAccident = 19,
            InvalidPayment = 20,
            MissingImages,
            IBANUsedForOtherDriver,
            InvalidEmail,
            InvalidPhone,
            PaidPolicy,
            PolicyFileGeneraionFailure,
            FailSendingMail,
            EmptyFiles,
            ResponseFail,
            ResponseEmpty,
            ZeroSearchResults,
            ZeroAddresses,
            policyInfo_is_null = 40,
            PolicyNo_is_empty = 41,
            ReferenceId_is_empty = 42,
            PolicyIssuanceDate_is_empty = 43,
            PolicyEffectiveDate_is_empty = 44,
            PolicyExpiryDate_is_empty = 45,
            PolicyFile_is_empty = 46,
            checkoutDetails_is_null = 47,
            Policy_is_already_success = 48,
            failed_to_save_policy = 49,
            ResponsePending = 50,
            DuplicateUserName = 51,
            GenerateRequestXMLFailed = 52,
            DuplicateSponsorId = 53,
            PasswordTooShort = 54,
            DuplicateCrNumber = 55,
            Failed = 56,
            BorderNumberIsNull = 57,
            NullResponse = 58,
            IqamaNumberIsNull = 59,
            IncompleteData = 60,
            RepeatedMobile = 61,
            NoMobileUpdated = 62,
            UnAuthoriztation = 63,
            ServiceError = 64,
            AuthorizationFailed = 65,
            FailedToFetch = 66,
            InvalidToken = 66,
            HashedNotMatched = 67,
            InvalidVatNumber = 68,
            InvalidData = 69,
            Processed_On_now = 74,
            AlreadyPaied = 75,           
            QuotationResponseIsNull =76,
            CompanyIsNull = 77,             
            PolicyFileDownloadFailure=78,
            InvoiceIsNull = 79,
            ChangePassword = 80,
            CompanyNotExist=81,
            NoValidCulture=82
        }
        public double ResponseTimeInSeconds { get; set; }
        public ErrorCodes ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        [JsonIgnore]
        public string LogDescription { get; set; }

        public TResult Result { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Interfaces.Localization.AllMessageKeys
{
    //For KeyName Localization
    public enum MessageKeysAccount
    {
        PhoneNumberOrPasswordIsWrong,
        AccountIsLocked,
        PhoneNumberAndAccountIsNotConfirmed,
        PhoneNumberOrOTPCodeIsWrong,
        PhoneNumberIsNotFound,
        CodeEnteredIsIncorrect,
        RefreshTokenIsNotFound,
        RefreshTokenIsExpire,
        EmailIsAlreadyUsed,
        BodySmsMessageToEnterTheCode,
        BodySmsMessageToOTP,
        VerificationCodeSentToPhoneNumber,
        BodySmsMessageForgetPassword,
        NewPasswordForForgetPasswordSentToPhoneNumber,
        UserIdNotFound

    }
}

using System.ComponentModel;

namespace Payment.Demo.Core.Enums
{
    public enum PaymentStatus
    {
        AUTHORIZATION_EXPIRED,
        AUTHORIZED,
        AUTHORIZING,
        FAILED,
        GATEWAY_REJECTED,
        PROCESSOR_DECLINED,
        SETTLED,
        SETTLING,
        SUBMITTED_FOR_SETTLEMENT,
        VOIDED,
        UNRECOGNIZED,
        SETTLEMENT_CONFIRMED,
        SETTLEMENT_DECLINED,
        SETTLEMENT_PENDING,

        //Custom status
        UKNOWN,
        ERRORFROMGATEWAY,
        PENDING,
    }
}

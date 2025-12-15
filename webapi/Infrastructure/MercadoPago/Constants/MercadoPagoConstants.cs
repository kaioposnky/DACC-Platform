namespace DaccApi.Infrastructure.MercadoPago.Constants
{
    public static class MercadoPagoConstants
    {
        public static class WebhookTypes
        {
            public const string Payment = "payment";
            public const string MerchantOrder = "merchant_order";
            public const string Plan = "plan";
            public const string Subscription = "subscription";
            public const string Invoice = "invoice";
        }

        public static class WebhookActions
        {
            public const string PaymentCreated = "payment.created";
            public const string PaymentUpdated = "payment.updated";
            public const string MerchantOrderUpdated = "merchant_order.updated";
        }

        public static class PaymentStatus
        {
            public const string Pending = "pending";
            public const string Approved = "approved";
            public const string Authorized = "authorized";
            public const string InProcess = "in_process";
            public const string InMediation = "in_mediation";
            public const string Rejected = "rejected";
            public const string Cancelled = "cancelled";
            public const string Refunded = "refunded";
        }

        public static class PaymentMethods
        {
            public const string Pix = "pix";
            public const string Visa = "visa";
            public const string Master = "master";
            public const string Amex = "amex";
            public const string Elo = "elo";
            public const string Boleto = "bolbradesco";
        }

        public static class PaymentTypes
        {
            public const string CreditCard = "credit_card";
            public const string DebitCard = "debit_card";
            public const string BankTransfer = "bank_transfer";
            public const string Ticket = "ticket";
            public const string AccountMoney = "account_money";
        }
    }
}
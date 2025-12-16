namespace DaccApi.Infrastructure.MercadoPago.Constants
{
    /// <summary>
    /// Contém constantes relacionadas à integração com o Mercado Pago.
    /// </summary>
    public static class MercadoPagoConstants
    {
        /// <summary>
        /// Define os tipos de webhooks do Mercado Pago.
        /// </summary>
        public static class WebhookTypes
        {
            public const string Payment = "payment";
            public const string MerchantOrder = "merchant_order";
            public const string Plan = "plan";
            public const string Subscription = "subscription";
            public const string Invoice = "invoice";
        }

        /// <summary>
        /// Define as ações de webhooks do Mercado Pago.
        /// </summary>
        public static class WebhookActions
        {
            public const string PaymentCreated = "payment.created";
            public const string PaymentUpdated = "payment.updated";
            public const string MerchantOrderUpdated = "merchant_order.updated";
        }

        /// <summary>
        /// Define os status de pagamento do Mercado Pago.
        /// </summary>
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

        /// <summary>
        /// Define os métodos de pagamento do Mercado Pago.
        /// </summary>
        public static class PaymentMethods
        {
            public const string Pix = "pix";
            public const string Visa = "visa";
            public const string Master = "master";
            public const string Amex = "amex";
            public const string Elo = "elo";
            public const string Boleto = "bolbradesco";
        }

        /// <summary>
        /// Define os tipos de pagamento do Mercado Pago.
        /// </summary>
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
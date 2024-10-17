using Common;
using DTOs.Invoice;
using Net.payOS.Errors;
using Net.payOS.Types;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Exceptions;
using Service.Interface;
using Service.PayOS;

namespace Service.Implement
{
    public class PayOSService : IPayOSService
    {
        private const string PAYMENT_REQUEST_URL = "https://api-merchant.payos.vn/v2/payment-requests/";

        public async Task<string> CreatePaymentLink(string invoiceId, int invoiceTimeStamp, int amount, string urlCancel, string urlReturn)
        {
            var client = ConfigurationHelper.config.GetSection("PayOS:ClientID").Value;
            var apiKey = ConfigurationHelper.config.GetSection("PayOS:APIKey").Value;
            var checkSumKey = ConfigurationHelper.config.GetSection("PayOS:CheckSumKey").Value;

            Net.payOS.PayOS payOS = new Net.payOS.PayOS(client, apiKey, checkSumKey);

            var item = new ItemData(invoiceId, 1, amount);
            List<ItemData> items = [item];

            int testAmount = amount / 1000;
            if (testAmount < 10000)
            {
                testAmount = 10000;
            }

            PaymentData paymentData = new PaymentData(invoiceTimeStamp, testAmount, $"For {amount} VND",
                                                                                            items, urlCancel, urlReturn);

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
            return createPayment.checkoutUrl;
        }

        public async Task<TransactionReturn> HandleCodeAfterPaymentQR(string code)
        {
            try
            {
                var client = ConfigurationHelper.config.GetSection("PayOS:ClientID").Value;
                var apiKey = ConfigurationHelper.config.GetSection("PayOS:APIKey").Value;
                var checkSumKey = ConfigurationHelper.config.GetSection("PayOS:CheckSumKey").Value;

                Net.payOS.PayOS payOS = new Net.payOS.PayOS(client, apiKey, checkSumKey);
                PaymentLinkInformation paymentLinkInformation = await GetPaymentLinkInformation(code);
                var inf = paymentLinkInformation.transactions.FirstOrDefault();
                if (paymentLinkInformation.status != "PAID")
                {
                    throw new ServiceException(MessageConstant.PayOS.PaymentReferenceError + paymentLinkInformation.status);
                }

                var bankAccounts = GetBankAccount();
                var bank = bankAccounts.Result.FirstOrDefault(x => x.bin == inf.counterAccountBankId);
                var transaction = new TransactionReturn()
                {
                    AccountName = inf?.counterAccountName,
                    AccountNumber = inf?.counterAccountNumber,
                    Amount = inf.amount * 1000,
                    BankCode = bank?.code,
                    BankName = bank?.shortName,
                    Reference = inf?.reference,
                    Description = inf?.description,
                    TransactionDate = DateTime.Parse(inf.transactionDateTime)
                };
                return transaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<BankAccount>> GetBankAccount()
        {
            var bankData = await File.ReadAllTextAsync("BankAccount.json");

            var banks = System.Text.Json.JsonSerializer.Deserialize<List<BankAccount>>(bankData);
            return banks;
        }

        public async Task<PaymentLinkInformation> GetPaymentLinkInformation(string payOsOrderId)
        {
            var client = ConfigurationHelper.config.GetSection("PayOS:ClientID").Value;
            var apiKey = ConfigurationHelper.config.GetSection("PayOS:APIKey").Value;

            string url = PAYMENT_REQUEST_URL + payOsOrderId;
            HttpClient httpClient = new HttpClient();
            JObject responseBodyJson = JObject.Parse(await (await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers =
            {
                { "x-client-id", client },
                { "x-api-key", apiKey }
            }
            })).Content.ReadAsStringAsync());
            string code = responseBodyJson["code"]?.ToString();
            string desc = responseBodyJson["desc"]?.ToString();
            string data = responseBodyJson["data"]?.ToString();
            if (code == null)
            {
                throw new PayOSError("20", "Internal Server Error.");
            }

            if (code == "00" && data != null)
            {
                PaymentLinkInformation response = JsonConvert.DeserializeObject<PaymentLinkInformation>(data);
                if (response == null)
                {
                    throw new InvalidOperationException("Error deserializing JSON response: Deserialized object is null.");
                }

                return response;
            }

            throw new PayOSError(code, desc);
        }


    }
}

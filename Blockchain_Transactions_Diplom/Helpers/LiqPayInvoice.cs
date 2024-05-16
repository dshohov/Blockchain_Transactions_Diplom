using Blockchain_Transactions_Diplom.IHelpers;
using Microsoft.Extensions.Options;
using PostmarkDotNet.Model;
using PostmarkDotNet;
using LiqPay.SDK.Dto.Enums;
using LiqPay.SDK.Dto;
using LiqPay.SDK;

namespace Blockchain_Transactions_Diplom.Helpers
{
    public class LiqPayInvoice : ILiqPayInvoice
    {
        private int CoinСost = 40;
        public LiqPayOptions Options { get; set; }//API
        public LiqPayInvoice(IOptions<LiqPayOptions> options)
        {
            Options = options.Value;
        }
        public async Task SendInvoiceAsync(string toEmail, int countCoins, string orderId)
        {
            if (string.IsNullOrEmpty(Options.PublicKey))
            {
                throw new Exception("Null Public Liq Pay Key");
            }
            if (string.IsNullOrEmpty(Options.PrivateKey))
            {
                throw new Exception("Null Private Liq Pay Key");
            }
            await Execute(Options.PublicKey, Options.PrivateKey, countCoins, toEmail, orderId);
        }
        private async Task Execute(string publicKey,string privateKey, int countCoins, string toEmail, string orderId)
        {
            var list_prod_for_check = new List<LiqPayRequestGoods>() { };
            var set_prod_for_check = new LiqPayRequestGoods() { Amount = CoinСost, Count = countCoins, Name = "Coins", Unit = "шт." };
            list_prod_for_check.Add(set_prod_for_check);

            // send invoce by email
            var invoiceRequest = new LiqPayRequest
            {
                Email = toEmail,
                Amount = countCoins * CoinСost,
                Currency = "UAH",
                OrderId = orderId,
                Action = LiqPayRequestAction.InvoiceSend,
                Language = LiqPayRequestLanguage.UK,
                Goods = list_prod_for_check

            }; 
            
            var liqPayClient = new LiqPayClient(publicKey, privateKey);
            //liqPayClient.IsCnbSandbox = true;
            var response = await liqPayClient.RequestAsync("request", invoiceRequest);

        }
        public async Task<bool> CheckInvoiceAsync(string orderId)
        {
            // send invoce by email
            var invoiceRequest2 = new LiqPayRequest
            {
                Version = 3,
                OrderId = orderId,
                Action = LiqPayRequestAction.Status,
            };
            var liqPayClient = new LiqPayClient(Options.PublicKey, Options.PrivateKey);
            //liqPayClient.IsCnbSandbox = true;
            var response = await liqPayClient.RequestAsync("request", invoiceRequest2);
            if(response.Status == LiqPayResponseStatus.Success)
                return true;
            return false;
        }
    }
}

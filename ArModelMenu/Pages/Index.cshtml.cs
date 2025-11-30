using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;

namespace ArModelMenu.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            // Sayfa yüklendiðinde yapýlacak iþler (Boþ kalabilir)
        }

        // --- ÝLETÝÞÝM FORMU ÝÇÝN HANDLER ---
        public JsonResult OnPostContact(string name, string email, string subject, string message)
        {
            try
            {
                // Mail Ýçeriðini Hazýrla
                string body = $"Ad: {name}<br>Email: {email}<br>Konu: {subject}<br>Mesaj: {message}";
                SendEmail("Yeni Ýletiþim Mesajý", body);

                return new JsonResult("OK"); // JS tarafý "OK" bekler
            }
            catch (Exception ex)
            {
                return new JsonResult($"Hata: {ex.Message}");
            }
        }

        // --- REZERVASYON FORMU ÝÇÝN HANDLER ---
        public JsonResult OnPostBookTable(string name, string email, string phone, string date, string time, string people, string message)
        {
            try
            {
                // Mail Ýçeriðini Hazýrla
                string body = $@"
                    <h3>Yeni Masa Rezervasyonu</h3>
                    <b>Ad:</b> {name}<br>
                    <b>Email:</b> {email}<br>
                    <b>Telefon:</b> {phone}<br>
                    <b>Tarih:</b> {date}<br>
                    <b>Saat:</b> {time}<br>
                    <b>Kiþi Sayýsý:</b> {people}<br>
                    <b>Mesaj:</b> {message}";

                SendEmail("Yeni Rezervasyon Ýsteði", body);

                return new JsonResult("OK");
            }
            catch (Exception ex)
            {
                return new JsonResult($"Hata: {ex.Message}");
            }
        }

        // --- ORTAK MAÝL GÖNDERME FONKSÝYONU ---
        private void SendEmail(string subject, string body)
        {
            var settings = _configuration.GetSection("EmailSettings");

            using (var client = new SmtpClient())
            {
                client.Host = settings["MailServer"];
                client.Port = int.Parse(settings["MailPort"]);
                client.EnableSsl = true; // SSL kullanýyorsan true, yoksa false
                client.Credentials = new NetworkCredential(settings["SenderEmail"], settings["SenderPassword"]);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(settings["SenderEmail"], settings["SenderName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                // Mail kime gidecek? (Yönetici mail adresin)
                mailMessage.To.Add("senin_mail_adresin@ornek.com");

                client.Send(mailMessage);
            }
        }
    }
}

using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace _28._01ui.Classes
{
    public static class ConfirmCode
    {
		public static void GenerateConfirmationCode()
		{
			var random = new Random();
			LastGeneratedCode = random.Next(100000, 999999).ToString();
		}

		public static string LastGeneratedCode { get; set; }

		public static async Task SendConfirmationEmail(string email, string code)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("FastLaneWorldConfirmation", "fastlaneworld@yandex.ru"));
			message.To.Add(new MailboxAddress("", email));
			message.Subject = "Код подтверждения FastLane World";
			message.Body = new TextPart("html")
			{
				Text = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{
                font-family: 'Segoe UI', Arial, sans-serif;
                line-height: 1.6;
                color: #333;
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
            }}
            .header {{
                text-align: center;
                padding-bottom: 20px;
                border-bottom: 1px solid #eaeaea;
                margin-bottom: 30px;
            }}
            .logo {{
                max-width: 150px;
                height: auto;
            }}
            .code {{
                font-size: 24px;
                font-weight: bold;
                color: #dc7a95;
                text-align: center;
                margin: 25px 0;
                padding: 15px;
                background: #f8f9fa;
                border-radius: 6px;
                letter-spacing: 2px;
            }}
            .footer {{
                margin-top: 30px;
                padding-top: 20px;
                border-top: 1px solid #eaeaea;
                font-size: 12px;
                color: #777;
                text-align: center;
            }}
        </style>
    </head>
    <body>
        <div class='header'>
            <img src='https://i.ibb.co/99vLrkww/flworldcommunity.png' alt=""FastLane World"" style=""display: block; margin: 0 auto; margin-top: 20px; margin-bottom: 50px; width: 450px;"">

			<h2>Подтверждение покупки билета</h2>
        </div>
        
        <p>Здравствуйте!</p>
        <p>Для завершения покупки билета на мероприятие FastLane World используйте следующий код подтверждения:</p>
        
        <div class='code'>{code}</div>
        
        <p>Код действителен в течение 3 минут. Никому не сообщайте этот код.</p>
        
        <div class='footer'>
            <p>Если вы не совершали эту покупку, проигнорируйте это письмо.</p>
            <p>© {DateTime.Now.Year} FastLane World. Все права защищены.</p>
        </div>
    </body>
    </html>"
			};

			using (var client = new SmtpClient())
			{
				await client.ConnectAsync("smtp.yandex.ru", 465, true);
				await client.AuthenticateAsync("fastlaneworld@yandex.ru", "adxffiuuemqujtfl");
				await client.SendAsync(message);
				await client.DisconnectAsync(true);
			}
		}
	}
}

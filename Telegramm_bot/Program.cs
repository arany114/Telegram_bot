using Microsoft.VisualBasic;
using System;
using System.Threading;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;


class Program
{
    static void Main()
    {
        var client = new TelegramBotClient("NO_API_KEY_FOR_YOU_😢");
        client.StartReceiving(Upd, Err);
        Console.ReadLine();
    }
    async static Task Upd(ITelegramBotClient botClient, Update upd, CancellationToken token)
    {
        var message = upd.Message;


        if (message != null)
        {
            Random random = new Random();
            int randomNumber = random.Next(200000);


            Console.WriteLine($"Имя отправителя: {message.Chat.FirstName + " " + message.Chat.LastName ?? " кто это"} | Текст сообщения: {message.Text}");
            switch (message.Text.ToLower())
            {
                case string text when text == "/start".ToLower():
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text:"/topic + аргумент (не более 2х слов)", "/topic"),

                            InlineKeyboardButton.WithCallbackData(text : "/randomphoto", "/randomphoto"),

                        },


                    }); ;
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Меню",
                        replyMarkup: inlineKeyboard,
                        cancellationToken: token);
                    break;
                case string text when text == "/randomphoto".ToLower():
                    string link = "https://source.unsplash.com/random/?Random&";
                    string info;
                    info = link + randomNumber.ToString();
                    await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: InputFile.FromUri(info),
                    caption: "<b>Picture</b>. <i>Source</i>: <a href=\"https://unsplash.com\">Unsplash</a>",
                    parseMode: ParseMode.Html,
                    cancellationToken: token);
                    break;
                case string text when text.StartsWith("/topic"):
                    string topic = text.Substring("/topic".Length).Trim();
                    string[] words = topic.Split(' ');
                    if (words.Length <= 2)
                    {
                        topic = topic.Replace(" ", "+");
                        string link1 = $"https://source.unsplash.com/random/?{topic}&{randomNumber}";
                        await botClient.SendPhotoAsync(
                        chatId: message.Chat.Id,
                        photo: InputFile.FromUri(link1),
                        caption: $"<b>Picture</b>.<i>Source</i>: <a href = \"{link1}\">Unsplash</a>",
                        parseMode: ParseMode.Html,
                        cancellationToken: token);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Слишком много букв! больше двух слов не перевариваю");
                    }

                    break;
                default:
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Такой комманды я не знаю!");

                    break;
            }

        }


    }
    private static Task Err(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }
}
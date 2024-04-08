using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StationFallHelperBot
{
    internal class MainBOt
    {
        public bool IsActive = true;
        public static ITelegramBotClient Bot;
        public static long admin = 0; //TODO
        public static StationFallData data;
        public string[] chars = new string[] {
"ASTROCHIMP",
"COUNSELOR",
"CYBORG",
"DAREDEVIL",
"ENGINEER",
"EXILE",
"MEDICAL",
"SECURITY",
"STATION CHIEF",
"STOWAWAY",
"STRANGER",
"TROUBLESHOOTER"
        };

        public MainBOt() {            
            Bot = new TelegramBotClient(Environment.GetEnvironmentVariable("BotToken"));
            data = JsonConvert.DeserializeObject<StationFallData>(System.IO.File.ReadAllText("data.json"));
            data.Characters = data.Characters.Where(x => chars.Contains(x.name)).ToArray();
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            Bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }

        private async Task HandleErrorAsync(ITelegramBotClient arg1, Exception exception, CancellationToken arg3)
        {
            SendLog($"Ошибка бота {Newtonsoft.Json.JsonConvert.SerializeObject(exception)}", true);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                SendLog(Newtonsoft.Json.JsonConvert.SerializeObject(update));

                switch (update.Type)
                {
                    case UpdateType.Unknown:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.Message:
                        if (update.Message.Text == "/characters") {
                            List<InlineKeyboardButton> list = new List<InlineKeyboardButton>();
                            list = data.Characters.Select(x => InlineKeyboardButton.WithCallbackData(text: x.name, callbackData: x.name)).ToList();
                            InlineKeyboardMarkup repl = CreateInlineMarkup(list, 2);
                            var m = Bot.SendTextMessageAsync(update.Message.From.Id, "Выберете персонажа", parseMode: ParseMode.Html, replyMarkup: repl).Result;
                        }
                        //RegisterUser(update.Message.From);
                        //await ProcessTextType(botClient, update);
                        //throw new NotImplementedException();
                        return;
                    case UpdateType.InlineQuery:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.ChosenInlineResult:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.CallbackQuery:
                        var c = data.Characters.Where(x => x.name == update.CallbackQuery.Data).FirstOrDefault();
                        var x = Bot.SendTextMessageAsync(update.CallbackQuery.From.Id, c.ToHTMLString(), parseMode: ParseMode.Html).Result;
                        //CheckForDialog(update.CallbackQuery.From);
                        //dialogs[update.CallbackQuery.From.Id].HandleCallbackQuery(update.CallbackQuery, cancellationToken);
                        //if (GetActiveDialog(update.CallbackQuery.Message.Chat.Id) == null) { return; }
                        //await bot.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, InlineKeyboardMarkup.Empty());
                        //SetActiveDialog(update.CallbackQuery.Message.Chat.Id, GetActiveDialog(update.CallbackQuery.Message.Chat.Id).DoAction(update));
                        return;
                    case UpdateType.EditedMessage:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.ChannelPost:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.EditedChannelPost:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.ShippingQuery:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.PreCheckoutQuery:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.Poll:
                        /*if (update.Poll.IsClosed)
                        {
                            ProcessClosedPoll(update);
                        }*/
                        throw new NotImplementedException();
                        break;
                    case UpdateType.PollAnswer:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.MyChatMember:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.ChatMember:
                        throw new NotImplementedException();
                        break;
                    case UpdateType.ChatJoinRequest:
                        throw new NotImplementedException();
                        break;
                    default:
                        break;
                }
                return;
            }
            catch (Exception e)
            {
                SendLog($"Ошибка {e}", true);
                IsActive = false;
            }
        }

        private void SendLog(string str, bool withAdminNotification = false)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {str}");
            if (withAdminNotification)
            {
                SendToAdmin(str);
            }
        }

        private void SendToAdmin(string str)
        {
            Bot.SendTextMessageAsync(admin, str);
        }
        protected InlineKeyboardMarkup CreateInlineMarkup(List<InlineKeyboardButton> list, int itemsInRowCount)
        {

            List<InlineKeyboardButton> l1 = new List<InlineKeyboardButton>();
            List<List<InlineKeyboardButton>> l2 = new List<List<InlineKeyboardButton>>();
            foreach (var item in list)
            {
                if (l1.Count == itemsInRowCount)
                {
                    l2.Add(l1);
                    l1 = new List<InlineKeyboardButton>();
                }
                l1.Add(item);
            }
            l2.Add(l1);
            return new InlineKeyboardMarkup(l2);
        }
    }
}


using DiscordBotCSharp.MessageDesigns;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotCSharp
{
    public class Bot
    {

        public DiscordClient Client { get; private set; }
        public InteractivityExtension interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public static List<DiscordChannel> ChannelsForAnnouncements =new List<DiscordChannel>();

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))//Your File where you have to place Token and Prefix
            {
                using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                {
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            var configJson = JsonConvert.DeserializeObject<configjson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            this.Client = new DiscordClient(config);

            this.Client.Ready += OnClientReady;
            this.Client.GuildDownloadCompleted += OnBotGuildsIsLoaded;

            this.Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(0.25),
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,
                IgnoreExtraArguments = false,
            };

            this.Commands = this.Client.UseCommandsNext(commandsConfig);

            this.Commands.RegisterCommands<CMDs>();

            await this.Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnBotGuildsIsLoaded(DiscordClient sender, GuildDownloadCompletedEventArgs e)
        {
            foreach (var guilds in sender.Guilds.Values)
            {
                foreach (var channel in guilds.Channels.Values)
                {
                    if (channel.Name.ToLower().Contains(TextFragments.ANNOUNCEMENTS.ToLower()))
                    {
                        var embed = new DesignFactory().GetEmbed(TextFragments.ANNOUNCEMENTS, TextFragments.BOT_ONLINE_ANNOUNCEMENT, setAuthor:false);
                        channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                        ChannelsForAnnouncements.Add(channel);
                        break;
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task OnClientReady(object t, ReadyEventArgs e)
        {
            if (t is DiscordClient discordClient)
            {
                DiscordActivity discordStatus = new DiscordActivity();
                discordStatus.Name = "Type ;info";
                discordStatus.ActivityType = ActivityType.Playing;

                discordClient.UpdateStatusAsync(discordStatus);

                Debug.WriteLine("BotPing: " + discordClient.Ping);//TODO DEBUG Logg
                Debug.WriteLine("Login: " + DateTime.Now.ToString());//TODO DEBUG Logg
                Debug.WriteLine("Bot Staus set to: " + discordStatus.Name);//TODO DEBUG Logg
            }

            return Task.CompletedTask;
        }
    }
}
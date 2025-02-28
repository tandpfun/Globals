﻿using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Globals.Data;
using Globals.Global;
using Globals.Util;
using Data;

namespace Globals
{
    class CommandHandler
    {
        private CommandService commands;
        private static DiscordSocketClient bot;
        private IServiceProvider map;

        public CommandHandler(IServiceProvider provider)
        {
            map = provider;
            bot = map.GetService<DiscordSocketClient>();

            bot.JoinedGuild += HandleJoinGuildAsync;
            bot.LeftGuild += HandleLeftGuildAsync;

            bot.Ready += ReadyAsync;
            bot.MessageReceived += HandleCommandAsync;
            //bot.MessageReceived += HandleGlobalMessageAsync;

            commands = map.GetService<CommandService>();
        }

        private async Task HandleJoinGuildAsync(SocketGuild guild)
        {
            await ServerConfig.RegisterServerAsync(guild.Id);
            await UpdateStatus();
        }

        private async Task HandleLeftGuildAsync(SocketGuild guild)
        {
            await ServerConfig.UnregisterServerAsync(guild.Id);
            await UpdateStatus();
        }

        private async Task HandleCommandAsync(SocketMessage pMsg)
        {
            SocketUserMessage message = pMsg as SocketUserMessage;
            if (message == null) return;
            var context = new SocketCommandContext(bot, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix(BotConfig.Load().BotPrefix, ref argPos))
            {
                var result = await commands.ExecuteAsync(context, argPos, map);
                if (!result.IsSuccess && result.ErrorReason != "Unknown command.") Console.WriteLine(result.ErrorReason);
            }
            else
            {
                if (!ProfanityFilter.HasProfanity(pMsg.Content.ToString()))
                {
                    await HandleGlobalMessageAsync(pMsg);
                }
                else
                {
                    var dbCon = DBConnection.Instance();
                    dbCon.DatabaseName = BotConfig.Load().DatabaseName;
                    if (dbCon.IsConnect())
                    {
                        await pMsg.DeleteAsync();
                        await Message.DeleteAsync(pMsg.Author, dbCon);
                        await UserProfile.AddWarningAsync(pMsg.Author, dbCon);
                        dbCon.Close();
                    }
                }
            }
        }

        private async Task HandleGlobalMessageAsync(SocketMessage pMsg)
        {
            SocketUserMessage message = pMsg as SocketUserMessage;
            if (message == null) return;
            var context = new SocketCommandContext(bot, message);
            if (message.Author.IsBot) return;

            await Message.PostGlobalMessageAsync(context);
        }

        private async Task ReadyAsync()
        {
            await GlobalMessages.ClearData();
            await UpdateStatus();
        }

        private async Task UpdateStatus()
        {
            await bot.SetGameAsync("Connecting " + bot.Guilds.Count + " communities.");
            await bot.SetStatusAsync(UserStatus.Idle);
        }

        public async Task ConfigureAsync() { await commands.AddModulesAsync(Assembly.GetEntryAssembly(), map); }

        public static DiscordSocketClient GetBot()
        {
            return bot;
        }
    }
}
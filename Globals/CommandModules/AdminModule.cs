﻿using Discord;
using Discord.Commands;
using Globals.Data;
using Globals.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Globals.CommandModules
{
    public class AdminModule : ModuleBase
    {
        [Command("setup")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task RegisterAsync()
        {
            await Context.Message.DeleteAsync();

            await ServerConfig.RegisterServerAsync(Context.Guild.Id);
            var Message = await Context.Channel.SendMessageAsync("Setup started, use the command `!enable` next.");
            await Delete.DeleteMessage(Message);
        }

        [Command("enable")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task EnableAsync(string channel = "", bool toggle = true)
        {
            await Context.Message.DeleteAsync();

            if (channel.ToLower().Equals("gaming")) await ServerConfig.ToggleChannel(Context.Guild.Id, "gaming", toggle);
            else if (channel.ToLower().Equals("music")) await ServerConfig.ToggleChannel(Context.Guild.Id, "music", toggle);
            else if (channel.ToLower().Equals("movies")) await ServerConfig.ToggleChannel(Context.Guild.Id, "movies", toggle);
            else if (channel.ToLower().Equals("rainbow6")) await ServerConfig.ToggleChannel(Context.Guild.Id, "rainbowsix", toggle);
            else if (channel.ToLower().Equals("league")) await ServerConfig.ToggleChannel(Context.Guild.Id, "league", toggle);
            else if (channel.ToLower().Equals("rust")) await ServerConfig.ToggleChannel(Context.Guild.Id, "rust", toggle);
            else if (channel.ToLower().Equals("gta")) await ServerConfig.ToggleChannel(Context.Guild.Id, "gta", toggle);
            else if (channel.ToLower().Equals("pubg")) await ServerConfig.ToggleChannel(Context.Guild.Id, "pubg", toggle);
            else if (channel.ToLower().Equals("fortnite")) await ServerConfig.ToggleChannel(Context.Guild.Id, "fortnite", toggle);
            else if (channel.ToLower().Equals("apex")) await ServerConfig.ToggleChannel(Context.Guild.Id, "apex", toggle);

            string GamingState = "Disabled", MusicState = "Disabled", MoviesState = "Disabled", R6State = "Disabled", LeagueState = "Disabled", RustState = "Disabled", GtaState = "Disabled", PubgState = "Disabled", FortniteState = "Disabled", ApexState = "Disabled";
            ServerConfig.GetChannelSettingsAsText(Context.Guild.Id, ref GamingState, ref MusicState, ref MoviesState, ref R6State, ref LeagueState, ref RustState, ref GtaState, ref PubgState, ref FortniteState, ref ApexState);

            var embed = new EmbedBuilder() { Color = new Color(114, 137, 218) };
            embed.WithDescription("Enable/Disable channels using `!enable <channel> <true/false>`, for example `!enable rainbow6 true`.\nOnce the correct channels are set, use the command `!create`, if this is the initial setup or `!update`, if the bot is already setup on your server.");
            embed.AddField(new EmbedFieldBuilder() { Name = "Gaming", Value = GamingState });
            embed.AddField(new EmbedFieldBuilder() { Name = "Music", Value = MusicState });
            embed.AddField(new EmbedFieldBuilder() { Name = "Movies", Value = MoviesState });
            embed.AddField(new EmbedFieldBuilder() { Name = "Rainbow6", Value = R6State });
            embed.AddField(new EmbedFieldBuilder() { Name = "League", Value = LeagueState });
            embed.AddField(new EmbedFieldBuilder() { Name = "Rust", Value = RustState });
            embed.AddField(new EmbedFieldBuilder() { Name = "GTA", Value = GtaState });
            embed.AddField(new EmbedFieldBuilder() { Name = "PUBG", Value = PubgState });
            embed.AddField(new EmbedFieldBuilder() { Name = "Fortnite", Value = FortniteState });
            embed.AddField(new EmbedFieldBuilder() { Name = "Apex", Value = ApexState });
            embed.WithCurrentTimestamp();

            var Message = await Context.Channel.SendMessageAsync(null, false, embed.Build());
            await Delete.DeleteMessage(Message);
        }

        [Command("create")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task CreateAsync()
        {
            await Context.Message.DeleteAsync();

            // Create globals category
            var cat = await Context.Guild.CreateCategoryAsync("Globals");

            // Create all channels
            bool GamingState = false, MusicState = false, MoviesState = false, R6State = false, LeagueState = false, RustState = false, GtaState = false, PubgState = false, FortniteState = false, ApexState = false;
            ulong GamingId = 0, MusicId = 0, MoviesId = 0, R6Id = 0, LeagueId = 0, RustId = 0, GtaId = 0, PubgId = 0, FortniteId = 0, ApexId = 0;
            ServerConfig.GetChannelSettingsAsBool(Context.Guild.Id, ref GamingState, ref MusicState, ref MoviesState, ref R6State, ref LeagueState, ref RustState, ref GtaState, ref PubgState, ref FortniteState, ref ApexState);

            if (GamingState == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("Gaming");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                GamingId = chan.Id;
            }

            if (MusicState == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("Music");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                MusicId = chan.Id;
            }

            if (MoviesState == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("Movies");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                MoviesId = chan.Id;
            }

            if (R6State == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("Rainbow Six");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                R6Id = chan.Id;
            }

            if (LeagueState == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("League of Legends");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                LeagueId = chan.Id;
            }

            if (RustState == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("Rust");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                RustId = chan.Id;
            }

            if (GtaState == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("GTA");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                GtaId = chan.Id;
            }

            if (PubgState == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("PUBG");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                PubgId = chan.Id;
            }

            if (FortniteState == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("Fortnite");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                FortniteId = chan.Id;
            }

            if (ApexState == true)
            {
                var chan = await Context.Guild.CreateTextChannelAsync("Apex");
                await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                ApexId = chan.Id;
            }

            await ServerConfig.SetupChannels(Context.Guild.Id, GamingId, MusicId, MoviesId, R6Id, LeagueId, RustId, GtaId, PubgId, FortniteId, ApexId);

            var Message = await Context.Channel.SendMessageAsync("Setup completed, use the `!update` command again to disable/enable channels in the future, once you disable or enable servers, use the `!update` command.");
            await Delete.DeleteMessage(Message);
        }

        [Command("update")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task UpdateAsync()
        {
            await Context.Message.DeleteAsync();

            // Check what channels have been disabled.
            // Delete any that have been disabled.

            // Check what channels have been enabled.
            // Create any that have been enabled.

            var cats = Context.Guild.GetCategoriesAsync().Result;
            ICategoryChannel cat = null;
            foreach (var category in cats)
            {
                if (category.Name.ToLower().Equals("globals"))
                {
                    cat = category;
                }
            }

            if (cat != null)
            {
                bool GamingState = false, MusicState = false, MoviesState = false, R6State = false, LeagueState = false, RustState = false, GtaState = false, PubgState = false, FortniteState = false, ApexState = false;
                ulong GamingId = 0, MusicId = 0, MoviesId = 0, R6Id = 0, LeagueId = 0, RustId = 0, GtaId = 0, PubgId = 0, FortniteId = 0, ApexId = 0;
                ServerConfig.GetChannelSettingsAsBool(Context.Guild.Id, ref GamingState, ref MusicState, ref MoviesState, ref R6State, ref LeagueState, ref RustState, ref GtaState, ref PubgState, ref FortniteState, ref ApexState);
                ServerConfig.GetChannelIds(Context.Guild.Id, ref GamingId, ref MusicId, ref MoviesId, ref R6Id, ref LeagueId, ref RustId, ref GtaId, ref PubgId, ref FortniteId, ref ApexId);

                if (GamingState == true && GamingId == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("Gaming");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    GamingId = chan.Id;
                }
                else if (GamingState == false && GamingId != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(GamingId);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        GamingId = 0;
                    }
                }

                if (MusicState == true && MusicId == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("Music");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    MusicId = chan.Id;
                }
                else if (MusicState == false && MusicId != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(MusicId);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        MusicId = 0;
                    }
                }

                if (MoviesState == true && MoviesId == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("Movies");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    MoviesId = chan.Id;
                }
                else if (MoviesState == false && MoviesId != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(MoviesId);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        MoviesId = 0;
                    }
                }

                if (R6State == true && R6Id == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("Rainbow Six");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    R6Id = chan.Id;
                }
                else if (R6State == false && R6Id != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(R6Id);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        R6Id = 0;
                    }
                }

                if (LeagueState == true && LeagueId == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("League");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    LeagueId = chan.Id;
                }
                else if (LeagueState == false && LeagueId != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(LeagueId);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        LeagueId = 0;
                    }
                }

                if (RustState == true && RustId == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("Rust");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    RustId = chan.Id;
                }
                else if (RustState == false && RustId != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(RustId);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        RustId = 0;
                    }
                }

                if (GtaState == true && GtaId == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("GTA");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    GtaId = chan.Id;
                }
                else if (GtaState == false && GtaId != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(GtaId);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        GtaId = 0;
                    }
                }

                if (PubgState == true && PubgId == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("PUBG");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    PubgId = chan.Id;
                }
                else if (PubgState == false && PubgId != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(PubgId);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        PubgId = 0;
                    }
                }

                if (FortniteState == true && FortniteId == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("Fortnite");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    FortniteId = chan.Id;
                }
                else if (FortniteState == false && FortniteId != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(FortniteId);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        FortniteId = 0;
                    }
                }

                if (ApexState == true && ApexId == 0)
                {
                    var chan = await Context.Guild.CreateTextChannelAsync("Apex");
                    await chan.ModifyAsync(x => x.CategoryId = cat.Id);
                    ApexId = chan.Id;
                }
                else if (ApexState == false && ApexId != 0)
                {
                    var chan = await Context.Guild.GetChannelAsync(ApexId);
                    if (chan != null)
                    {
                        await chan.DeleteAsync();
                        ApexId = 0;
                    }
                }

                await ServerConfig.SetupChannels(Context.Guild.Id, GamingId, MusicId, MoviesId, R6Id, LeagueId, RustId, GtaId, PubgId, FortniteId, ApexId);

                var message = await Context.Channel.SendMessageAsync("Your global channels should now be updated. Please use the `!request` command in a global channel, if you have any issues.");
                await Delete.DeleteMessage(message);
            }
            else
            {
                var message = await Context.Channel.SendMessageAsync("We couldn't find the globals category in your server, I suggest deleting all the global channels and category. Then run the `!setup` command again.");
                await Delete.DeleteMessage(message);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using BotsMod;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CustomCharacters;

public class DiscordController : MonoBehaviour
{

	public Discord.Discord discord;

	// Use this for initialization
	void Start()
	{
		try
		{
			discord = new Discord.Discord(793142591637684244, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

			var activityManager = discord.GetActivityManager();


			var activity = new Discord.Activity
			{
				State = "someone fucking help me please",
				Details = "Game Starting",
				Assets = new ActivityAssets()
				{
					LargeImage = "etggif",
					LargeText = "",
					SmallImage = "",
					SmallText = "",
				}
			};
			activityManager.UpdateActivity(activity, (res) =>
			{
				if (res == Discord.Result.Ok)
				{
					BotsModule.Log("Everything is fine!");
					
					
				}
				else
				{
					BotsModule.Log("Everything is **NOT** fine!");
				}
			});
		}
		catch (Exception e)
		{
			BotsModule.Log("Discord stuff did a broken now you can cry", "#eb1313");
			BotsModule.Log(string.Format(e + ""), "#eb1313");
		}


	}



	private string GetPlayerName(PlayerController player)
	{
		switch (player.characterIdentity)
		{
			case PlayableCharacters.Bullet:
				return "Bullet";
				
			case PlayableCharacters.Convict:
				return "Convict";
				
			case PlayableCharacters.CoopCultist:
				return "CoopCultist";
				
			case PlayableCharacters.Cosmonaut:
				return "Cosmonaut";
				
			case PlayableCharacters.Eevee:
				return "Eevee";
				
			case PlayableCharacters.Guide:
				return "Guide";
				
			case PlayableCharacters.Gunslinger:
				return "Gunslinger";
				
			case PlayableCharacters.Ninja:
				return "Ninja";
				
			case PlayableCharacters.Pilot:
				return "Pilot";
				
			case PlayableCharacters.Robot:
				return "Robot";
				
			case PlayableCharacters.Soldier:
				return "Soldier";
				
			case (PlayableCharacters)CustomPlayableCharacters.Lost:
				return "11";

			default:
				return "Pilot";	
		}
	}
	public dfLabel label;
	// Update is called once per frame
	void Update()
	{
		


		try
		{
			//GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TOTAL_MONEY_COLLECTED).ToString();
			var activityManager = discord.GetActivityManager();
			if (GameManager.Instance.PrimaryPlayer != null && !GameManager.Instance.IsLoadingLevel)
			{
				var player = GameManager.Instance.PrimaryPlayer;

				var activity = new Discord.Activity
				{
					State = "someone fucking help me please",
					Details = this.ForceGetLocalizedValue(GameManager.Instance.Dungeon.DungeonShortName),
					Assets = new ActivityAssets()
					{
						LargeImage = "etggif",
						LargeText = "Gun Go Pew Pew",
						SmallImage = GetPlayerName(player).ToLower(),

						SmallText = StringTableManager.EvaluateReplacementToken("%PLAYER_NAME"),//StringTableManager.GetString(GetTalkingPlayerName()),
					}
				};
				activityManager.UpdateActivity(activity, (res) =>
				{
					if (res != Discord.Result.Ok)
					{
						BotsModule.Log("Everything is **NOT** fine!");
					}
				});

			}

			discord.RunCallbacks();
		}
		catch (Exception e)
		{
			BotsModule.Log("Discord (update) stuff did a broken now you can cry", "#eb1313");
			BotsModule.Log(string.Format(e + ""), "#eb1313");
		}
	}

	public string ForceGetLocalizedValue(string key)
	{
		dfLanguageManager component = GameUIRoot.Instance.GetComponent<dfLanguageManager>();
		return component.GetValue(key);
	}

	private static PlayerController GetTalkingPlayer()
	{
		List<TalkDoerLite> allNpcs = StaticReferenceManager.AllNpcs;
		for (int i = 0; i < allNpcs.Count; i++)
		{
			if (allNpcs[i])
			{
				if (!allNpcs[i].IsTalking || !allNpcs[i].TalkingPlayer || GameManager.Instance.HasPlayer(allNpcs[i].TalkingPlayer))
				{
					if (allNpcs[i].IsTalking && allNpcs[i].TalkingPlayer)
					{
						return allNpcs[i].TalkingPlayer;
					}
				}
			}
		}
		return GameManager.Instance.PrimaryPlayer;
	}

	private static string GetTalkingPlayerName()
	{
		PlayerController talkingPlayer = GetTalkingPlayer();
		if (talkingPlayer.IsThief)
		{
			return "#THIEF_NAME";
		}
		if (talkingPlayer.characterIdentity == PlayableCharacters.Eevee)
		{
			return "#PLAYER_NICK_RANDOM";
		}
		if (talkingPlayer.characterIdentity == PlayableCharacters.Gunslinger)
		{
			return "#PLAYER_NICK_GUNSLINGER";
		}
		return "#PLAYER_NAME_" + talkingPlayer.characterIdentity.ToString().ToUpperInvariant();
	}
}
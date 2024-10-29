﻿using GDWeave.Godot.Variants;
using GDWeave.Godot;
using GDWeave.Modding;

namespace Sulayre.Lure.Patches
{
	public class NetworkPatcher : IScriptMod
	{
		public bool ShouldRun(string path) => path == "res://Scenes/Singletons/SteamNetwork.gdc";

		// returns a list of tokens for the new script, with the input being the original script's tokens
		public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
		{
			// wait for any newline token after any extends token

			var waiter = new MultiTokenWaiter([
				t => t.Type is TokenType.Comma,
				t => t is IdentifierToken{Name:"ver"},
				t => t.Type is TokenType.ParenthesisClose,
				t => t.Type is TokenType.Newline,

			],	allowPartialMatch: false);

			var waitercode = new MultiTokenWaiter([
				t => t.Type is TokenType.OpAssign,
				t => t is IdentifierToken{Name:"LOBBY"},
				t => t.Type is TokenType.Newline,

			], allowPartialMatch: false);

			// loop through all tokens in the script
			foreach (var token in tokens)
			{
				if (waiter.Check(token))
				{
					yield return token;
					yield return new IdentifierToken("ver");
					yield return new Token(TokenType.OpAssign);
					yield return new IdentifierToken("get_node");
					yield return new Token(TokenType.ParenthesisOpen);
					yield return new ConstantToken(new StringVariant("/root/SulayreLure/Patches"));
					yield return new Token(TokenType.ParenthesisClose);
					yield return new Token(TokenType.Period);
					yield return new IdentifierToken("_load_lobby_map");
					yield return new Token(TokenType.ParenthesisOpen);
					yield return new IdentifierToken("id");
					yield return new Token(TokenType.Comma);
					yield return new IdentifierToken("ver");
					yield return new Token(TokenType.ParenthesisClose);
					yield return new Token(TokenType.Newline,1);
				}
				else if (waitercode.Check(token))
				{
					yield return token;

					yield return new IdentifierToken("LOBBY_VERSION");
					yield return new Token(TokenType.OpAssign);
					yield return new IdentifierToken("get_node");
					yield return new Token(TokenType.ParenthesisOpen);
					yield return new ConstantToken(new StringVariant("/root/SulayreLure/Patches"));
					yield return new Token(TokenType.ParenthesisClose);
					yield return new Token(TokenType.Period);
					yield return new IdentifierToken("_filter_lobby_map");
					yield return new Token(TokenType.ParenthesisOpen);
					yield return new IdentifierToken("LOBBY");
					yield return new Token(TokenType.Comma);
					yield return new IdentifierToken("LOBBY_VERSION");
					yield return new Token(TokenType.ParenthesisClose);
					yield return new Token(TokenType.Newline, 2);
				}
				{
					// return the original token
					yield return token;
				}
			}
		}
	}
}
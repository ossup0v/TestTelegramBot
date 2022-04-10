using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Domain.Domain.OXPlay
{
    public sealed class OXGameStorage
    {
        private Dictionary<Guid, OXGame> _games = new Dictionary<Guid, OXGame>();
        public IReadOnlyDictionary<Guid, OXGame> Games => _games;
        public static OXGameStorage Instance { get; } = new OXGameStorage();

        public void AddGame(OXGame game)
        {
            _games.Add(game.Id, game);
        }

        [return: MaybeNull]
        public OXGame? GetGame(Guid id)
        {
            _games.TryGetValue(id, out var game);
            return game;
        }

        public void RemoveGame(Guid id)
        {
            _games.Remove(id);
        }
    }
}

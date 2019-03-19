using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw14
{
    class Game
    {
        public List<Card> AllCards { get; set; }
        public List<Player> AllPlayer { get; set; }
        public List<Card> CardsInGame { get; set; }

        public Game(int playersInGame)
        {
            if (playersInGame > 1 && playersInGame < 7)
            {
                AllCards = new List<Card>();
                AllPlayer = new List<Player>();
                CardsInGame = new List<Card>();

                for (int i = 0; i < playersInGame; i++)
                {
                    AllPlayer.Add(new Player((ConsoleColor)i+9));
                }

                foreach (var suit in new[] { CardSuits.Diamonds, CardSuits.Hearts, CardSuits.Spades, CardSuits.Clubs })
                {
                    foreach (var type in new[] { CardTypes.Six, CardTypes.Seven, CardTypes.Eight, CardTypes.Nine, CardTypes.Ten, CardTypes.Jack, CardTypes.Queen, CardTypes.King, CardTypes.Ace })
                    {
                        AllCards.Add(new Card { CardTypes = type, CardSuits = suit });
                    }
                }
                ShuffleCards();

                CardsForPlayers(playersInGame);
            }
        }

        public void CardsForPlayers(int playersInGame)
        {
            int numberOfPlayerCards = AllCards.Count / playersInGame;

            for (int i = 0; i < numberOfPlayerCards; i++)
            {
                for (int j = 0; j < playersInGame; j++)
                {
                    AllPlayer[j].PlayerCards.Add(AllCards.Last());
                    AllCards.Remove(AllCards.Last());
                }
            }
        }
    
        public void ShuffleCards()
        {
            Random random = new Random();

            for (int i = 0; i < AllCards.Count; i++)
            {
                var temp = AllCards[i];
                AllCards.RemoveAt(i);
                AllCards.Insert(random.Next(AllCards.Count), temp);
            }
        }
        
        public void ShowAllCards()
        {
            foreach (var card in AllCards)
            {
                Console.WriteLine(card.CardSuits +" "+ card.CardTypes);
            }
        }
        public void ShowPlayersCards()
        {
            int firstIndex = 0;
            int indent = 0; 
            foreach (var player in AllPlayer)
            {
                int secondIndex = 2;
                Console.SetCursorPosition(indent, 0);
                Console.WriteLine($"|Player : {firstIndex + 1}");
                Console.SetCursorPosition(indent, 1);
                Console.WriteLine("------------------");
                foreach (var card in player.PlayerCards)
                {
                    if (card != null)
                    {
                        Console.SetCursorPosition(indent, secondIndex);
                        Console.WriteLine($"|{secondIndex - 1} {card.CardSuits}: {card.CardTypes}");
                        secondIndex++;
                    }
                }
                indent += 19;
                firstIndex++;
            }
        }

        public void StartGame()
        {
            while (true)
            {
                int maxCards = GetMaxCards();
                ShowPlayersCards();

                int index = 0;
                foreach (var player in AllPlayer)
                {
                    if (player.PlayerCards.Count > 0)
                    {
                        Console.ForegroundColor = player.Color;
                        Console.SetCursorPosition(AllPlayer.Count*19/2-6, maxCards + 3);
                        Console.WriteLine($"Ход игрока {index + 1}");
                        Console.SetCursorPosition(index * 19, maxCards + 5);

                        int playerSelection;
                        if (int.TryParse(Console.ReadLine(), out playerSelection))
                        {
                            if (playerSelection > 0 && playerSelection <= player.PlayerCards.Count)
                            {
                                Console.SetCursorPosition(index * 19, playerSelection+1);
                                Console.WriteLine($"|{playerSelection} {player.PlayerCards[playerSelection - 1].CardSuits}: {player.PlayerCards[playerSelection - 1].CardTypes}" );

                                player.ThrownCard = player.PlayerCards[playerSelection - 1];
                                player.PlayerCards.RemoveAt(playerSelection - 1);
                            }
                        }
                        Console.ResetColor();
                    }
                    index++;
                }
                Console.Clear();

                int indexWinner = GetIndexWinner();

                Winner(indexWinner);
                index = 0;

                if (GetMaxCards()+ AllCards.Count == 36)
                {
                    Console.WriteLine($"Победил игрок {indexWinner + 1}");
                    Console.ReadLine();
                    break;
                }
            }
        }

        public void Winner(int indexWinner)
        {
            foreach (var player in AllPlayer)
            {
                if (player.ThrownCard != null)
                {
                    AllPlayer[indexWinner].PlayerCards.Add(player.ThrownCard);
                    player.ThrownCard = null;
                }
            }
        }

        public int GetMaxCards()
        {
            int maxCards = 0;
            foreach (var player in AllPlayer)
            {
                if (player.PlayerCards.Count > maxCards)
                    maxCards = player.PlayerCards.Count;
            }
            return maxCards;
        }

        public int GetIndexWinner()
        {
            int indexWinner = 0, maxCardType = 0, index = 0;
            foreach (var player in AllPlayer)
            {
                if (player.ThrownCard != null && (int)player.ThrownCard.CardTypes > maxCardType)
                {
                    maxCardType = (int)player.ThrownCard.CardTypes;
                    indexWinner = index;
                }
                index++;
            }
            return indexWinner;
        }
    }
}

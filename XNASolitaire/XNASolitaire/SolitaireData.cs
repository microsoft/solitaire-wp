/*
 * Copyright (c) 2011 Nokia Corporation.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNASolitaire
{

    /// <summary>
    /// List of SolitaireData classes for tombstoning
    /// </summary>
    public class SolitaireDataList
    {
        public List<SolitaireData> m_list = new List<SolitaireData>();

        public SolitaireDataList()
        {
        }

        /// <summary>
        /// Add Deck list into SolitaireDataList for tombstoning
        /// </summary>
        /// <param name="fromDeckList"></param>
        public void AddDeckData(List<Deck> fromDeckList)
        {
            for (int i = 0; i < fromDeckList.Count(); i++)
            {
                Deck deck = fromDeckList[i];
                for (int j = 0; j < deck.CardCount(); j++)
                {
                    Card card = deck.GetCard(j);
                    SolitaireData data = new SolitaireData();
                    data.AddCardData(card, deck.InternalDeckId());
                    m_list.Add(data);
                }
            }
        }

        /// <summary>
        /// Create Deck list from SolitaireDataList
        /// </summary>
        /// <param name="toDeckList"></param>
        /// <param name="allCards"></param>
        public void ReadDeckDataToList(ref List<Deck> toDeckList, ref List<Card> allCards)
        {
            Deck userDeck1 = null;
            Deck userDeck2 = null;
            Deck userDeck3 = null;
            Deck userDeck4 = null;
            Deck userDeck5 = null;
            Deck userDeck6 = null;
            Deck userDeck7 = null;

            TargetDeck targetDeck1 = null;
            TargetDeck targetDeck2 = null;
            TargetDeck targetDeck3 = null;
            TargetDeck targetDeck4 = null;

            SourceDeck sourceDeck = null;
            WasteDeck wasteDeck = null;

            // Find right decks, have to use internal deck id for matching
            for (int i = 0; i < toDeckList.Count(); i++)
            {
                Deck deck = toDeckList[i];
                if (deck.InternalDeckId() == 10)
                    userDeck1 = deck;
                else if (deck.InternalDeckId() == 20)
                    userDeck2 = deck;
                else if (deck.InternalDeckId() == 30)
                    userDeck3 = deck;
                else if (deck.InternalDeckId() == 40)
                    userDeck4 = deck;
                else if (deck.InternalDeckId() == 50)
                    userDeck5 = deck;
                else if (deck.InternalDeckId() == 60)
                    userDeck6 = deck;
                else if (deck.InternalDeckId() == 70)
                    userDeck7 = deck;
                else if (deck.InternalDeckId() == 100)
                    targetDeck1 = (TargetDeck)deck;
                else if (deck.InternalDeckId() == 200)
                    targetDeck2 = (TargetDeck)deck;
                else if (deck.InternalDeckId() == 300)
                    targetDeck3 = (TargetDeck)deck;
                else if (deck.InternalDeckId() == 400)
                    targetDeck4 = (TargetDeck)deck;
                else if (deck.InternalDeckId() == 1000)
                    sourceDeck = (SourceDeck)deck;
                else if (deck.InternalDeckId() == 2000)
                    wasteDeck = (WasteDeck)deck;
            }

            // Add cards into right decks
            for (int i = 0; i < m_list.Count(); i++)
            {
                // Tombstoned card data list
                SolitaireData data = m_list[i]; 

                // Find card class
                Card c = findCard(ref allCards, data.m_land, data.m_id);
                if (c == null)
                    break;
                c.setTurned(data.m_isTurned);

                // Set card into right deck
                switch (data.m_internalDeckId)
                {
                    case 10:
                        {
                            userDeck1.AddCard(c);
                            break;
                        }
                    case 20:
                        {
                            userDeck2.AddCard(c);
                            break;
                        }
                    case 30:
                        {
                            userDeck3.AddCard(c);
                            break;
                        }
                    case 40:
                        {
                            userDeck4.AddCard(c);
                            break;
                        }
                    case 50:
                        {
                            userDeck5.AddCard(c);
                            break;
                        }
                    case 60:
                        {
                            userDeck6.AddCard(c);
                            break;
                        }
                    case 70:
                        {
                            userDeck7.AddCard(c);
                            break;
                        }
                    case 100:
                        {
                            targetDeck1.AddCard(c);
                            break;
                        }
                    case 200:
                        {
                            targetDeck2.AddCard(c);
                            break;
                        }
                    case 300:
                        {
                            targetDeck3.AddCard(c);
                            break;
                        }
                    case 400:
                        {
                            targetDeck4.AddCard(c);
                            break;
                        }
                    case 1000:
                        {
                            sourceDeck.AddCard(c);
                            break;
                        }
                    case 2000:
                        {
                            wasteDeck.AddCard(c);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Finds card using given land and id parameters
        /// </summary>
        /// <param name="allCards"></param>
        /// <param name="land"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private Card findCard(ref List<Card> allCards, int land, int id)
        {
            Card card = null;
            for (int i = 0; i < allCards.Count(); i++)
            {
                Card c = allCards[i];
                if (land == (int)c.CardLand() && id == c.CardId())
                {
                    card = c;
                    break;
                }
            }
            return card;
        }
    }

    /// <summary>
    /// Solitaire data class for tombstoning Solitaire card data.
    /// Class and all methods that will be serialized have to be public.
    /// </summary>
    public class SolitaireData
    {
        public int m_land;
        public int m_id;
        public bool m_isTurned = false;
        public bool m_isBlack = false;
        public int m_internalDeckId;
        public int m_z;

        public SolitaireData()
        {
            // Serialized class constructor cannot have parameters
        }

        /// <summary>
        /// Create SolitaireData from Card
        /// </summary>
        /// <param name="card"></param>
        /// <param name="internalDeckId"></param>
        public void AddCardData(Card card, int internalDeckId)
        {
            m_land = (int)card.CardLand();
            m_id = card.CardId();
            m_isBlack = card.IsBlack();
            m_isTurned = card.IsTurned();
            m_internalDeckId = internalDeckId;
            m_z = card.m_z;
        }
    }


}

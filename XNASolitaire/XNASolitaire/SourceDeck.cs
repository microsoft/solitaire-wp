/*
 * Copyright (c) 2011 Nokia Corporation.
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace XNASolitaire
{
    /// <summary>
    /// Source deck where user can only move card to Waste deck
    /// </summary>
    public class SourceDeck : TargetDeck
    {

        public WasteDeck p_wasteDeck;

        public SourceDeck(int internalDeckId)
        {
            m_internalDeckId = internalDeckId;
            m_deckType = Deck.DeckType.ESourceDeck;
        }

        ~SourceDeck()
        {
        }

        /// <summary>
        /// Handles Source deck touch. Moves card to Waste deck
        /// </summary>
        /// <param name="tl"></param>
        /// <returns></returns>
        public override Card HandleTouch(TouchLocation tl)
        {
            Card ret = null;

            foreach (Card c in m_cards)
            {
                if (c.isInTouch(tl))
                {
                    // Take most upper cards from the deck
                    if (ret != null && ret.m_z < c.m_z)
                    {
                        ret = c;
                    }
                    else if (ret == null)
                    {
                        ret = c;
                    }
                }
            }

            if (ret != null)
            {
                // If card found, turn it
                ret.setTurned(true);
                // and move to waste deck
                RemoveCard(ret);
                p_wasteDeck.AddCard(ret);
            }
            else
            {
                // Source deck is empty
                // Copy cards back to source deck from waste
                if (IsInTouch(tl) && CardCount() == 0)
                {
                    for (int i = 0; i < p_wasteDeck.CardCount(); i++)
                    {
                        Card lastCard = p_wasteDeck.RemoveLast();
                        lastCard.setTurned(false);
                        AddCard(lastCard);
                        i--;
                    }
                }

            }

            return null;
        }

    }

    /// <summary>
    /// Waste deck where user can take cards to the game
    /// </summary>
    public class WasteDeck : TargetDeck
    {

        public WasteDeck(int internalDeckId)
        {
            m_internalDeckId = internalDeckId;
            m_deckType = Deck.DeckType.EWasteDeck;
        }

        ~WasteDeck()
        {
        }

    }
}

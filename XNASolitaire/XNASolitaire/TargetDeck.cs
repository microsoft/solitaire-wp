/*
 * Copyright (c) 2011 Nokia Corporation.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace XNASolitaire
{
    /// <summary>
    /// Target deck where user tries to move cards
    /// </summary>
    public class TargetDeck : Deck
    {
        public TargetDeck()
        {
            m_deckType = Deck.DeckType.ETargetDeck;
        }

        public TargetDeck(int internalDeckId)
        {
            m_internalDeckId = internalDeckId;
            m_deckType = Deck.DeckType.ETargetDeck;
        }

        ~TargetDeck()
        {
        }

        public override void AddParentCard(Card parent)
        {
            // No impl, have to be empty
        }

        /// <summary>
        /// Adds new card to this deck
        /// </summary>
        /// <param name="newCard"></param>
        public override void AddCard(Card newCard)
        {
            // All cards in the deck are in same position
            Rectangle r = newCard.CardRectangle;
            Point p = m_pos;
            r.Location = p;
            newCard.CardRectangle = r;

            newCard.m_z = Game1.nextZ();

            if (m_cards.Count() > 0)
            {
                if (newCard.ChildCard != null)
                {
                    // Clear previous child parent
                    newCard.ChildCard.ParentCard = null;
                }
            }
            else
            {
                // Deck is empty
                if (newCard.ChildCard != null)
                {
                    // Clear previous child parent
                    newCard.ChildCard.ParentCard = null;
                }
                newCard.ChildCard = null;
            }
            
            // Mark owner deck into card
            newCard.OwnerDeck = this;

            // Add card into this deck
            m_cards.Add(newCard);
        }

        /// <summary>
        /// Is touch under this deck?
        /// </summary>
        /// <param name="tl"></param>
        /// <returns></returns>
        public override bool IsInTouch(TouchLocation tl)
        {
            // Deck Rectangle is deck size
            Rectangle r = new Rectangle(
                m_pos.X, 
                m_pos.Y, 
                Card.CARD_WIDTH, 
                Card.CARD_HEIGHT);

            // Make touch area of the deck wider
            r.Inflate(Game1.DECK_TOUCH_ADDITION, Game1.DECK_TOUCH_ADDITION);

            if (r.Contains(new Point((int)tl.Position.X, (int)tl.Position.Y)))
                return true;

            return false;
        }

    }

}

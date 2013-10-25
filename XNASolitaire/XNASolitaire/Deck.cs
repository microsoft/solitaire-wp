/*
 * Copyright (c) 2011 Nokia Corporation.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace XNASolitaire
{
    /// <summary>
    /// Base deck class and base class for the SourceDeck, WasteDeck and TargetDeck
    /// </summary>
    public class Deck
    {
        // Cards of this deck
        protected List<Card> m_cards = new List<Card>();

        // Position of this deck on the screen
        protected Point m_pos;
        
        // Deck bacground property
        Texture2D m_background;

        // Deck types
        public enum DeckType
        {
            EUserDeck = 1,
            ETargetDeck = 2,
            ESourceDeck = 3,
            EWasteDeck = 4
        };

        protected int m_internalDeckId;

        protected DeckType m_deckType;

        public Deck()
        {
            m_deckType = Deck.DeckType.EUserDeck;
        }

        public Deck(int internalDeckId)
        {
            m_internalDeckId = internalDeckId;
            m_deckType = Deck.DeckType.EUserDeck;
        }

        ~Deck()
        {
        }

        public int InternalDeckId()
        {
            return m_internalDeckId;
        }

        /// <summary>
        /// Deck type
        /// </summary>
        /// <returns></returns>
        public DeckType Type()
        {
            return m_deckType;
        }

        /// <summary>
        /// Deck poition
        /// </summary>
        public virtual Point Position
        {
            get
            {
                return m_pos;
            }
            set
            {
                m_pos = value;
            }
        }

        /// <summary>
        /// Count of cards
        /// </summary>
        /// <returns></returns>
        public virtual int CardCount()
        {
            return m_cards.Count();
        }

        /// <summary>
        /// Top most card of this deck
        /// </summary>
        /// <returns></returns>
        public virtual Card GetLast()
        {
            return m_cards.Last();
        }

        /// <summary>
        /// Removes last card / top most card from this deck
        /// </summary>
        /// <returns></returns>
        public virtual Card RemoveLast()
        {
            Card card = m_cards.Last();
            m_cards.Remove(card);
            return card;
        }

        /// <summary>
        /// Returns selected card
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Card GetCard(int index)
        {
            return m_cards[index];
        }

        /// <summary>
        /// Adds new card to this deck
        /// </summary>
        /// <param name="newCard"></param>
        public virtual void AddCard(Card newCard)
        {
            // Cards are positioned with 20 pixcels cap
            Rectangle r = newCard.CardRectangle;
            Point p = m_pos;
            p.Y = p.Y + (Card.CARD_CAP * m_cards.Count());
            r.Location = p;
            newCard.CardRectangle = r;

            newCard.m_z = Game1.nextZ();

            if (m_cards.Count() > 0)
            {
                // Deck has cards
                Card lastCard = m_cards.Last();
                lastCard.ParentCard = newCard;

                if (newCard.ChildCard != null)
                {
                    // Clear previous child parent
                    newCard.ChildCard.ParentCard = null;
                    // Set new child card
                    newCard.ChildCard = lastCard;
                }
                else
                {
                    // Add new child card
                    newCard.ChildCard = lastCard;
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

            // Add also all parent cards into this deck
            if (newCard.ParentCard != null)
            {
                AddParentCard(newCard.ParentCard);
            }
        }

        /// <summary>
        /// Used when user moves many cards on the same time between decks
        /// </summary>
        /// <param name="parent"></param>
        public virtual void AddParentCard(Card parent)
        {
            // Add parent card to this deck

            // Cards are positioned with 20 pixcels cap
            Rectangle r = parent.CardRectangle;
            Point p = m_pos;
            p.Y = p.Y + (Card.CARD_CAP * m_cards.Count());
            r.Location = p;
            parent.CardRectangle = r;
            parent.m_z = Game1.nextZ();

            // Mark owner deck into card
            parent.OwnerDeck = this;

            // Add card into this deck
            m_cards.Add(parent);

            // Add also all parent cards into this deck
            if (parent.ParentCard != null)
            {
                AddParentCard(parent.ParentCard);
            }
        }

        /// <summary>
        /// Removes card from the deck
        /// </summary>
        /// <param name="c"></param>
        public virtual void RemoveCard(Card c)
        {
            // Remove card from the deck
            m_cards.Remove(c);

            // Remove also all parent cards from the this deck
            if (c.ParentCard != null)
            {
                RemoveCard(c.ParentCard);
            }
        }

        /// <summary>
        /// Is touch on this deck?
        /// </summary>
        /// <param name="tl"></param>
        /// <returns></returns>
        public virtual bool IsInTouch(TouchLocation tl)
        {
            // Deck Rectangle is deck and its cards sizes
            Rectangle r = new Rectangle(
                m_pos.X, 
                m_pos.Y, 
                Card.CARD_WIDTH, 
                Card.CARD_HEIGHT + Game1.CARD_CAP * m_cards.Count());

            // Make touch area of the deck wider
            r.Inflate(Game1.DECK_TOUCH_ADDITION, Game1.DECK_TOUCH_ADDITION);

            if (r.Contains(new Point((int)tl.Position.X, (int)tl.Position.Y)))
                return true;

            return false;
        }

        /// <summary>
        /// Handles touch
        /// </summary>
        /// <param name="tl"></param>
        /// <returns></returns>
        public virtual Card HandleTouch(TouchLocation tl)
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
            // If card found, handle touch
            if (ret != null)
            {
                ret.handleTouch(tl);
            }
            return ret;
        }

        /// <summary>
        /// Draw deck background and all cards of this deck
        /// </summary>
        /// <param name="theSpriteBatch"></param>
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            // Draw empty deck background
            Rectangle r = new Rectangle(m_pos.X, m_pos.Y, Card.CARD_WIDTH, Card.CARD_HEIGHT);
            if (m_background != null)
                theSpriteBatch.Draw(m_background, r, Color.White);

            // Draw cards
            for (int i = 0; i < m_cards.Count(); i++)
            {
                Card c = m_cards[i];
                c.Draw(theSpriteBatch);
            }
        }

        public virtual void LoadBackground(ContentManager theContentManager)
        {
            m_background = theContentManager.Load<Texture2D>("deck_background");
        }

    }
    
}

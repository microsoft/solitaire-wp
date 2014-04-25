/*
 * Copyright (c) 2011-2014 Microsoft Mobile.
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
    /// Solitaire card that has value, land and texture
    /// </summary>
    public class Card
    {
        public enum CardLandEnum
        {
            EClubs = 1,
            EDiamond = 2,
            EHeart = 3,
            ESpade = 4
        };
        CardLandEnum m_land;
        int m_id = 0; // 1=ace, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11=jack, 12=queen, 13=king

        public static int CARD_WIDTH = 80;
        public static int CARD_HEIGHT = 110;
        public static int CARD_CAP = 25;

        Texture2D m_card = null; // Card picture
        Texture2D m_cardBack = null; // Card back picture
        Rectangle m_rect; // Card rectangle
        Rectangle m_originRect; // for cancel card move back to original position

        Card p_parentCard; // Upper / parent card
        Card p_childCard; // Lower / child card

        public int m_z = 0;  // Card z-order

        Deck p_ownerDeck; // Deck that owns this card

        bool m_isTurned = false; // Is card face or background visible
        bool m_isBlack = false; // Card is black or red

        int m_xCap = 0;
        int m_yCap = 0;

        String m_cardName; // for loading texture content

        public Card(CardLandEnum land, int id, int z, ContentManager theContentManager)
        {
            m_z = z;
            p_parentCard = null;
            p_childCard = null;
            p_ownerDeck = null;

            // Black or red card?
            if (land == Card.CardLandEnum.EClubs || land == Card.CardLandEnum.ESpade)
                m_isBlack = true;

            this.LoadCard(land, id, theContentManager);
        }

        public Card(int land, int id, int z)
        {
            m_z = z;
            p_parentCard = null;
            p_childCard = null;
            p_ownerDeck = null;

            // Black or red card?
            if (land == (int)Card.CardLandEnum.EClubs || land == (int)Card.CardLandEnum.ESpade)
                m_isBlack = true;

        }

        ~Card()
        {
        }

        /// <summary>
        /// Card land
        /// </summary>
        /// <returns></returns>
        public CardLandEnum CardLand()
        {
            return m_land;
        }

        /// <summary>
        /// Card value / id
        /// </summary>
        /// <returns></returns>
        public int CardId()
        {
            return m_id;
        }

        /// <summary>
        /// Is this black card?
        /// </summary>
        /// <returns></returns>
        public bool IsBlack()
        {
            return m_isBlack;
        }

        /// <summary>
        /// Is this turned card?
        /// </summary>
        /// <returns></returns>
        public bool IsTurned()
        {
            return m_isTurned;
        }

        /// <summary>
        /// Returns parent card / upper card of this
        /// </summary>
        public Card ParentCard
        {
            // Upper / parent card
            get
            {
                return p_parentCard;
            }
            set
            {
                p_parentCard = value;
            }
        }

        /// <summary>
        /// Returns child card / lower card of this
        /// </summary>
        public Card ChildCard
        {
            // Lower / child card
            get
            {
                return p_childCard;
            }
            set
            {
                p_childCard = value;
            }
        }

        /// <summary>
        /// Returns owner deck of this
        /// </summary>
        public Deck OwnerDeck
        {
            get
            {
                return p_ownerDeck;
            }
            set
            {
                p_ownerDeck = value;
            }
        }

        /// <summary>
        /// Returns card rectangle
        /// </summary>
        public Rectangle CardRectangle
        {
            get
            {
                return m_rect;
            }
            set
            {
                m_rect = value;
            }
        }

        /// <summary>
        /// Sets the card either turned or not.
        /// </summary>
        /// <param name="turned">If true, the value of the card will be shown.</param>
        public void setTurned(bool turned)
        {
            m_isTurned = turned;
        }

        /// <summary>
        /// Loads card texture
        /// </summary>
        /// <param name="land"></param>
        /// <param name="id"></param>
        /// <param name="theContentManager"></param>
        public void LoadCard(CardLandEnum land, int id, ContentManager theContentManager)
        {
            m_id = id;
            m_land = land;

            // Set default pos(0) and size
            m_rect = new Rectangle(0, 0, CARD_WIDTH, CARD_HEIGHT);
            m_originRect = m_rect;

            // Card background
            m_cardBack = theContentManager.Load<Texture2D>("card_background");

            // Card foreground
            switch (m_land)
            {
                case CardLandEnum.EClubs:
                    {
                        m_cardName = "Club";
                        break;
                    }
                case CardLandEnum.EDiamond:
                    {
                        m_cardName = "Diamond";
                        break;
                    }
                case CardLandEnum.EHeart:
                    {
                        m_cardName = "Heart";
                        break;
                    }
                case CardLandEnum.ESpade:
                    {
                        m_cardName = "Spade";
                        break;
                    }
            }


            switch (m_id)
            {
                case 1:
                    {
                        m_cardName = String.Concat(m_cardName, "_ace");
                        break;
                    }
                case 2:
                    {
                        m_cardName = String.Concat(m_cardName, "_2");
                        break;
                    }
                case 3:
                    {
                        m_cardName = String.Concat(m_cardName, "_3");
                        break;
                    }
                case 4:
                    {
                        m_cardName = String.Concat(m_cardName, "_4");
                        break;
                    }
                case 5:
                    {
                        m_cardName = String.Concat(m_cardName, "_5");
                        break;
                    }
                case 6:
                    {
                        m_cardName = String.Concat(m_cardName, "_6");
                        break;
                    }
                case 7:
                    {
                        m_cardName = String.Concat(m_cardName, "_7");
                        break;
                    }
                case 8:
                    {
                        m_cardName = String.Concat(m_cardName, "_8");
                        break;
                    }
                case 9:
                    {
                        m_cardName = String.Concat(m_cardName, "_9");
                        break;
                    }
                case 10:
                    {
                        m_cardName = String.Concat(m_cardName, "_10");
                        break;
                    }
                case 11:
                    {
                        m_cardName = String.Concat(m_cardName, "_jack");
                        break;
                    }
                case 12:
                    {
                        m_cardName = String.Concat(m_cardName, "_queen");
                        break;
                    }
                case 13:
                    {
                        m_cardName = String.Concat(m_cardName, "_king");
                        break;
                    }
            }

            // Load resource
            m_card = theContentManager.Load<Texture2D>(m_cardName);
        }

        /// <summary>
        /// Is this card touched
        /// </summary>
        /// <param name="tl"></param>
        /// <returns></returns>
        public bool isInTouch(TouchLocation tl)
        {
            bool ret = false;
            if (m_rect.Contains(new Point((int)tl.Position.X, (int)tl.Position.Y)))
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Move card back to the original position
        /// </summary>
        public void cancelMove()
        {
            // Move card back to original position
            m_rect = m_originRect;

            // Move also parent card move
            if (p_parentCard != null)
                p_parentCard.cancelMove();
        }

        /// <summary>
        /// Process Pressed and Moved touch
        /// </summary>
        /// <param name="tl"></param>
        public void handleTouch(TouchLocation tl)
        {
            if (tl.State == TouchLocationState.Pressed)
            {
                m_originRect = m_rect;
                m_xCap = (int)tl.Position.X - m_rect.X;
                m_yCap = (int)tl.Position.Y - m_rect.Y;
            }
            else if (tl.State == TouchLocationState.Moved)
            {
                // Allow move card only from deck 1 and 4
                if ((p_ownerDeck.Type() == Deck.DeckType.EUserDeck || p_ownerDeck.Type() == Deck.DeckType.EWasteDeck))
                {
                    m_rect.X = (int)tl.Position.X - m_xCap;
                    m_rect.Y = (int)tl.Position.Y - m_yCap;
                }
            }

            // Handle press and move of also all turned parent cards
            if (p_parentCard != null && p_parentCard.m_isTurned)
            {
                p_parentCard.handleTouch(tl);
            }

        }

        /// <summary>
        /// Draw this card and all parents cards
        /// </summary>
        /// <param name="theSpriteBatch"></param>
        public void DrawMeAndParent(SpriteBatch theSpriteBatch)
        {
            // Me
            this.Draw(theSpriteBatch);
            // And parent to top of Me
            if (p_parentCard != null)
                p_parentCard.DrawMeAndParent(theSpriteBatch);

        }

        /// <summary>
        /// Draw only this card
        /// </summary>
        /// <param name="theSpriteBatch"></param>
        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (m_isTurned)
            {
                if (m_card != null)
                    theSpriteBatch.Draw(m_card, m_rect, Color.White);
            }
            else
            {
                if (m_cardBack != null)
                    theSpriteBatch.Draw(m_cardBack, m_rect, Color.White);
            }
        }
    }
}

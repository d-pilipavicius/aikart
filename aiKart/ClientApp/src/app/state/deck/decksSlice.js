import { createSlice } from '@reduxjs/toolkit';
import { CardStatus } from "../../../features/card/CardStatus";

export const decksSlice = createSlice({
  name: "decks",
  initialState: {
    decks: [],
  },
  reducers: {
    addDeck: (state, action) => {
      const { name, description } = action.payload;
      state.decks.push({ name, description, cards: [] });
    },
    addCardToDeck: (state, action) => {
      const { deckName, card } = action.payload;
      const deck = state.decks.find((deck) => deck.name === deckName);
      if (deck) {
        deck.cards.push({ ...card, status: CardStatus.UNANSWERED });
      }
    },
    editCardInDeck: (state, action) => {
      const { deckName, cardIndex, newCard } = action.payload;
      const deck = state.decks.find((deck) => deck.name === deckName);
      if (deck) {
        deck.cards[cardIndex] = newCard;
      }
    },
    deleteCardFromDeck: (state, action) => {
      const { deckName, cardIndex } = action.payload;
      const deck = state.decks.find((deck) => deck.name === deckName);
      if (deck) {
        deck.cards.splice(cardIndex, 1);
      }
    },
  },
});

export const { addDeck, addCardToDeck, editCardInDeck, deleteCardFromDeck } = decksSlice.actions;
export default decksSlice.reducer;

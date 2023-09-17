import { createSlice } from '@reduxjs/toolkit';

export const decksSlice = createSlice({
  name: 'decks',
  initialState: {
    decks: []
  },
  reducers: {
    addDeck: (state, action) => {
      state.decks.push(action.payload);
    }
  }
});

export const { addDeck } = decksSlice.actions;
export default decksSlice.reducer;

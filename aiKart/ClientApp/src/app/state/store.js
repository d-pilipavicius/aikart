import { configureStore } from '@reduxjs/toolkit';
import decksReducer from './deck/decksSlice';

const store = configureStore({
  reducer: {
    decks: decksReducer
  }
});

export default store; 
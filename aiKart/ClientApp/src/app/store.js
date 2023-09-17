import { configureStore } from '@reduxjs/toolkit';
import decksReducer from '../features/deck/decksSlice';

const store = configureStore({
  reducer: {
    decks: decksReducer
  }
});

export default store; 
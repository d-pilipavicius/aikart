// React redux global state management, this basically acts as a global state for the entire application.
// The store is a parent component that wraps the entire application, allowing all child components to access the store.
import { configureStore } from "@reduxjs/toolkit";
import decksReducer from "./deck/decksSlice";
import cardsReducer from "./card/cardsSlice";
import usersReducer from "./user/usersSlice";
import userDecksReducer from "./user/userDecksSlice";

const store = configureStore({
  reducer: {
    decks: decksReducer,
    cards: cardsReducer,
    users: usersReducer,
    userDecks: userDecksReducer,
  },
});

export default store;

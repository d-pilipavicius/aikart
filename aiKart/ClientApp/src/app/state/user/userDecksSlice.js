import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";

// Async thunk to fetch user decks
export const fetchUserDecks = createAsyncThunk("userDecks/fetchUserDecks", async () => {
  const response = await axios.get("/api/userdeck");
  return response.data;
});

// Async thunk to fetch decks by user
export const fetchDecksByUser = createAsyncThunk("userDecks/fetchDecksByUser", async (userId) => {
  const response = await axios.get(`/api/userdeck/${userId}/decks`);
  return response.data;
});

// Async thunk to fetch users of a deck
export const fetchUsersOfDeck = createAsyncThunk("userDecks/fetchUsersOfDeck", async (deckId) => {
  const response = await axios.get(`/api/userdeck/${deckId}/users`);
  return response.data;
});

// Async thunk to add user to a deck
export const addUserToDeck = createAsyncThunk("userDecks/addUserToDeck", async (userDeckDto) => {
  const response = await axios.post("/api/userdeck", userDeckDto);
  return response.data;
});

const userDeckSlice = createSlice({
  name: "userDecks",
  initialState: {
    userDecks: [],
    loading: "idle",
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchUserDecks.fulfilled, (state, action) => {
        state.loading = "idle";
        state.userDecks = action.payload;
      })
      .addCase(fetchUserDecks.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(fetchUserDecks.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(fetchDecksByUser.fulfilled, (state, action) => {
        state.loading = "idle";
        state.userDecks = action.payload;
      })
      .addCase(fetchDecksByUser.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(fetchDecksByUser.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(fetchUsersOfDeck.fulfilled, (state, action) => {
        state.loading = "idle";
        state.userDecks = action.payload;
      })
      .addCase(fetchUsersOfDeck.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(fetchUsersOfDeck.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(addUserToDeck.fulfilled, (state, action) => {
        state.loading = "idle";
        state.userDecks.push(action.payload);
      })
      .addCase(addUserToDeck.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(addUserToDeck.rejected, (state, action) => {
        state.loading = "idle";
      });
  },
});

export default userDeckSlice.reducer;

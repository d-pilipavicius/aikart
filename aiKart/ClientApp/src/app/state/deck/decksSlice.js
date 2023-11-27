import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";

export const fetchDecks = createAsyncThunk("decks/fetchDecks", async () => {
  const response = await axios.get("/api/deck/public");
  return response.data;
});

export const addDeck = createAsyncThunk("decks/addDeck", async (deckDto) => {
  const response = await axios.post("/api/deck", deckDto);
  return response.data;
});

export const updateDeck = createAsyncThunk(
  "decks/updateDeck",
  async ({ deckId, deckDto }) => {
    const response = await axios.put(`/api/deck/${deckId}`, deckDto);
    return response.data;
  }
);

export const deleteDeck = createAsyncThunk(
  "decks/deleteDeck",
  async (deckId) => {
    await axios.delete(`/api/deck/${deckId}`);
    return deckId;
  }
);

export const fetchDeckById = createAsyncThunk(
  "decks/fetchDeckById",
  async (deckId) => {
    const response = await axios.get(`/api/deck/${deckId}`);
    return response.data;
  }
);

export const generateDeck = createAsyncThunk(
  "decks/generateDeck",
  async (triviaDeckRequestDto) => {
    const response = await axios.post("/api/trivia", triviaDeckRequestDto);
    return response.data;
  }
);

export const decksSlice = createSlice({
  name: "decks",
  initialState: {
    decks: [],
    loading: "idle",
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchDecks.fulfilled, (state, action) => {
        state.loading = "idle";
        state.decks = action.payload;
      })
      .addCase(fetchDecks.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(fetchDecks.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(addDeck.fulfilled, (state, action) => {
        state.loading = "idle";
        state.decks.push(action.payload);
      })
      .addCase(addDeck.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(addDeck.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(updateDeck.fulfilled, (state, action) => {
        state.loading = "idle";
        const index = state.decks.findIndex(
          (deck) => deck.id === action.payload.id
        );
        if (index !== -1) {
          state.decks[index] = action.payload;
        }
      })
      .addCase(updateDeck.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(updateDeck.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(deleteDeck.fulfilled, (state, action) => {
        state.loading = "idle";
        const index = state.decks.findIndex(
          (deck) => deck.id === action.payload
        );
        if (index !== -1) {
          state.decks.splice(index, 1);
        }
      })
      .addCase(deleteDeck.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(deleteDeck.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(fetchDeckById.fulfilled, (state, action) => {
        state.loading = "idle";
        state.decks.push(action.payload);
      })
      .addCase(fetchDeckById.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(fetchDeckById.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(generateDeck.fulfilled, (state, action) => {
        state.loading = "idle";
        state.decks.push(action.payload);
      })
      .addCase(generateDeck.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(generateDeck.rejected, (state, action) => {
        state.loading = "idle";
      });
  },
});

export default decksSlice.reducer;

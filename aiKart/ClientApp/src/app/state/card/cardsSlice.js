import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";

export const fetchCardById = createAsyncThunk(
  "cards/fetchCardById",
  async (cardId) => {
    const response = await axios.get(`/api/card/${cardId}`);
    return response.data;
  }
);

export const addCard = createAsyncThunk("cards/addCard", async (cardDto) => {
  const response = await axios.post("/api/card", cardDto);
  return response.data;
});

export const updateCardState = createAsyncThunk(
  "cards/updateCardState",
  async ({ cardId, newState }) => {
    const response = await axios.put(`/api/card/states/set/${cardId}`, {
      state: newState,
    });
    return response.data;
  }
);

export const updateCard = createAsyncThunk(
  "cards/updateCard",
  async ({ cardId, cardDto }) => {
    const response = await axios.put(`/api/card/${cardId}`, cardDto);
    return response.data;
  }
);

export const deleteCard = createAsyncThunk(
  "cards/deleteCard",
  async (cardId) => {
    await axios.delete(`/api/card/${cardId}`);
    return cardId;
  }
);

export const cardsSlice = createSlice({
  name: "cards",
  initialState: {
    cards: [],
    loading: "idle",
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchCardById.fulfilled, (state, action) => {
        state.loading = "idle";
        state.cards.push(action.payload);
      })
      .addCase(fetchCardById.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(fetchCardById.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(updateCardState.fulfilled, (state, action) => {
        const index = state.cards.findIndex(
          (card) => card.id === action.payload.id
        );
        if (index !== -1) {
          state.cards[index] = action.payload;
        }
      })
      .addCase(updateCardState.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(updateCardState.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(addCard.fulfilled, (state, action) => {
        state.loading = "idle";
        state.cards.push(action.payload);
      })
      .addCase(addCard.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(addCard.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(updateCard.fulfilled, (state, action) => {
        state.loading = "idle";
        const index = state.cards.findIndex(
          (card) => card.id === action.payload.id
        );
        if (index !== -1) {
          state.cards[index] = action.payload;
        }
      })
      .addCase(updateCard.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(updateCard.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(deleteCard.fulfilled, (state, action) => {
        state.loading = "idle";
        const index = state.cards.findIndex(
          (card) => card.id === action.payload
        );
        if (index !== -1) {
          state.cards.splice(index, 1);
        }
      })
      .addCase(deleteCard.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(deleteCard.rejected, (state, action) => {
        state.loading = "idle";
      });
  },
});

export default cardsSlice.reducer;
